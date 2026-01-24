using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.Enums;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.Notification;
using JoyModels.Models.DataTransferObjects.RequestTypes.Order;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Order;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.Settings;
using JoyModels.Services.Services.Orders.HelperMethods;
using JoyModels.Services.Validation;
using JoyModels.Utilities.RabbitMQ.MessageProducer;
using Microsoft.EntityFrameworkCore;
using Stripe;
using LibraryEntity = JoyModels.Models.Database.Entities.Library;

namespace JoyModels.Services.Services.Orders;

public class OrderService(
    JoyModelsDbContext context,
    IMapper mapper,
    UserAuthValidation userAuthValidation,
    StripeSettingsDetails stripeSettings,
    IMessageProducer messageProducer)
    : IOrderService
{
    private readonly CustomerService _customerService = new();
    private readonly EphemeralKeyService _ephemeralKeyService = new();
    private readonly PaymentIntentService _paymentIntentService = new();

    public async Task<OrderCheckoutResponse> Checkout()
    {
        var userUuid = userAuthValidation.GetUserClaimUuid();

        var cartItems = await OrderHelperMethods.GetUserCartItemsWithModels(context, userUuid);

        if (cartItems.Count == 0)
            throw new ArgumentException("Shopping cart is empty.");

        var invalidModels = cartItems.Where(x => x.ModelUu.Price <= 0).Select(x => x.ModelUu.Name).ToList();
        if (invalidModels.Count > 0)
            throw new ArgumentException($"These models cannot be purchased: {string.Join(", ", invalidModels)}");

        await OrderHelperMethods.ValidateModelsNotAlreadyOwned(context, userUuid, cartItems);

        var modelUuids = cartItems.Select(x => x.ModelUuid).ToList();
        await context.Orders
            .Where(x => x.UserUuid == userUuid
                        && modelUuids.Contains(x.ModelUuid)
                        && x.Status == nameof(OrderStatus.Pending))
            .ExecuteDeleteAsync();

        var totalAmount = cartItems.Sum(x => x.ModelUu.Price);
        var amountInCents = (long)(totalAmount * 100);

        var user = await context.Users.FirstAsync(x => x.Uuid == userUuid);
        var customerId = await GetOrCreateStripeCustomer(user);

        var ephemeralKey = await _ephemeralKeyService.CreateAsync(new EphemeralKeyCreateOptions
        {
            Customer = customerId,
            StripeVersion = "2024-06-20"
        });

        var paymentIntent = await _paymentIntentService.CreateAsync(new PaymentIntentCreateOptions
        {
            Amount = amountInCents,
            Currency = stripeSettings.Currency,
            Customer = customerId,
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions { Enabled = true },
            Metadata = new Dictionary<string, string>
            {
                { "user_uuid", userUuid.ToString() }
            }
        });

        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            foreach (var cartItem in cartItems)
            {
                var order = new Order
                {
                    Uuid = Guid.NewGuid(),
                    UserUuid = userUuid,
                    ModelUuid = cartItem.ModelUuid,
                    Amount = cartItem.ModelUu.Price,
                    Status = nameof(OrderStatus.Pending),
                    StripePaymentIntentId = paymentIntent.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await context.Orders.AddAsync(order);
            }

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await _paymentIntentService.CancelAsync(paymentIntent.Id);
            throw;
        }

        return new OrderCheckoutResponse
        {
            ClientSecret = paymentIntent.ClientSecret,
            EphemeralKey = ephemeralKey.Secret,
            CustomerId = customerId,
            PaymentIntentId = paymentIntent.Id
        };
    }

    public async Task HandleWebhook(string json, string stripeSignature)
    {
        var stripeEvent = EventUtility.ConstructEvent(
            json, stripeSignature, stripeSettings.WebhookSecret);

        switch (stripeEvent.Type)
        {
            case EventTypes.PaymentIntentSucceeded:
                await HandlePaymentSucceeded(stripeEvent);
                break;

            case EventTypes.PaymentIntentPaymentFailed:
                await HandlePaymentFailed(stripeEvent);
                break;
        }
    }

    public async Task<OrderResponse> GetByUuid(Guid orderUuid)
    {
        var userUuid = userAuthValidation.GetUserClaimUuid();
        var orderEntity = await OrderHelperMethods.GetOrderEntity(context, userUuid, orderUuid);
        return mapper.Map<OrderResponse>(orderEntity);
    }

    public async Task<PaginationResponse<OrderResponse>> Search(OrderSearchRequest request)
    {
        var orderEntities = await OrderHelperMethods.SearchOrderEntities(context, userAuthValidation, request);
        return mapper.Map<PaginationResponse<OrderResponse>>(orderEntities);
    }

    public async Task<PaginationResponse<OrderResponse>> AdminSearch(OrderAdminSearchRequest request)
    {
        var orderEntities = await OrderHelperMethods.AdminSearchOrderEntities(context, request);
        return mapper.Map<PaginationResponse<OrderResponse>>(orderEntities);
    }

    private async Task<string> GetOrCreateStripeCustomer(User user)
    {
        if (!string.IsNullOrEmpty(user.StripeCustomerId))
            return user.StripeCustomerId;

        var customer = await _customerService.CreateAsync(new CustomerCreateOptions
        {
            Email = user.Email,
            Name = $"{user.FirstName} {user.LastName}".Trim(),
            Metadata = new Dictionary<string, string>
            {
                { "user_uuid", user.Uuid.ToString() }
            }
        });

        user.StripeCustomerId = customer.Id;
        await context.SaveChangesAsync();

        return customer.Id;
    }

    private async Task HandlePaymentSucceeded(Event stripeEvent)
    {
        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        if (paymentIntent == null) return;

        var orders = await context.Orders
            .Include(x => x.Model)
            .Where(x => x.StripePaymentIntentId == paymentIntent.Id)
            .ToListAsync();

        if (orders.Count == 0) return;

        if (orders.All(o => o.Status == nameof(OrderStatus.Completed)))
            return;

        var userUuid = orders.First().UserUuid;

        await using var transaction = await context.Database.BeginTransactionAsync();

        foreach (var order in orders)
        {
            order.Status = nameof(OrderStatus.Completed);
            order.UpdatedAt = DateTime.UtcNow;
        }

        foreach (var order in orders)
        {
            var existingLibrary = await context.Libraries
                .FirstOrDefaultAsync(x => x.UserUuid == order.UserUuid && x.ModelUuid == order.ModelUuid);

            if (existingLibrary != null) continue;

            var libraryEntry = new LibraryEntity
            {
                Uuid = Guid.NewGuid(),
                UserUuid = order.UserUuid,
                ModelUuid = order.ModelUuid,
                OrderUuid = order.Uuid,
                AcquiredAt = DateTime.UtcNow
            };
            await context.Libraries.AddAsync(libraryEntry);
        }

        var modelUuids = orders.Select(o => o.ModelUuid).ToList();
        await context.ShoppingCartItems
            .Where(x => x.UserUuid == userUuid && modelUuids.Contains(x.ModelUuid))
            .ExecuteDeleteAsync();

        await context.SaveChangesAsync();
        await transaction.CommitAsync();

        foreach (var order in orders)
        {
            var buyerNotification = new CreateNotificationRequest
            {
                ActorUuid = order.Model.UserUuid,
                TargetUserUuid = order.UserUuid,
                NotificationType = nameof(NotificationType.OrderCompleted),
                Title = "Purchase Complete",
                Message = $"You have successfully purchased '{order.Model.Name}'.",
                RelatedEntityUuid = order.Uuid,
                RelatedEntityType = "Order"
            };
            await messageProducer.SendMessage("create_notification", buyerNotification);

            if (order.Model.UserUuid != order.UserUuid)
            {
                var sellerNotification = new CreateNotificationRequest
                {
                    ActorUuid = order.UserUuid,
                    TargetUserUuid = order.Model.UserUuid,
                    NotificationType = nameof(NotificationType.ModelSold),
                    Title = "Model Sold",
                    Message = $"Your model '{order.Model.Name}' has been purchased.",
                    RelatedEntityUuid = order.ModelUuid,
                    RelatedEntityType = "Model"
                };
                await messageProducer.SendMessage("create_notification", sellerNotification);
            }
        }
    }

    private async Task HandlePaymentFailed(Event stripeEvent)
    {
        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        if (paymentIntent == null) return;

        var orders = await context.Orders
            .Include(x => x.Model)
            .Where(x => x.StripePaymentIntentId == paymentIntent.Id)
            .ToListAsync();

        if (orders.Count == 0) return;

        foreach (var order in orders)
        {
            order.Status = nameof(OrderStatus.Failed);
            order.UpdatedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();

        var userUuid = orders.First().UserUuid;
        var modelNames = string.Join(", ", orders.Select(o => $"'{o.Model.Name}'"));

        var notification = new CreateNotificationRequest
        {
            ActorUuid = userUuid,
            TargetUserUuid = userUuid,
            NotificationType = nameof(NotificationType.OrderFailed),
            Title = "Payment Failed",
            Message = $"Your payment for {modelNames} has failed. Please try again.",
            RelatedEntityUuid = orders.First().Uuid,
            RelatedEntityType = "Order"
        };
        await messageProducer.SendMessage("create_notification", notification);
    }
}