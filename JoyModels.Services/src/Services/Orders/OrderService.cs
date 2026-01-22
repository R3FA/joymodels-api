using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.Enums;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.Order;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Order;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.Settings;
using JoyModels.Services.Services.Orders.HelperMethods;
using JoyModels.Services.Validation;
using Microsoft.EntityFrameworkCore;
using Stripe;
using LibraryEntity = JoyModels.Models.Database.Entities.Library;

namespace JoyModels.Services.Services.Orders;

public class OrderService(
    JoyModelsDbContext context,
    IMapper mapper,
    UserAuthValidation userAuthValidation,
    StripeSettingsDetails stripeSettings)
    : IOrderService
{
    private readonly CustomerService _customerService = new();
    private readonly EphemeralKeyService _ephemeralKeyService = new();
    private readonly PaymentIntentService _paymentIntentService = new();

    public async Task<OrderCheckoutResponse> Checkout()
    {
        var userUuid = userAuthValidation.GetUserClaimUuid();

        // 1. Get all cart items for user
        var cartItems = await OrderHelperMethods.GetUserCartItemsWithModels(context, userUuid);

        if (cartItems.Count == 0)
            throw new ArgumentException("Shopping cart is empty.");

        // 2. Validate all models have a price > 0
        var invalidModels = cartItems.Where(x => x.ModelUu.Price <= 0).Select(x => x.ModelUu.Name).ToList();
        if (invalidModels.Count > 0)
            throw new ArgumentException($"These models cannot be purchased: {string.Join(", ", invalidModels)}");

        // 3. Check if user already owns any of these models
        await OrderHelperMethods.ValidateModelsNotAlreadyOwned(context, userUuid, cartItems);

        // 4. Cancel any existing pending orders for these models (cleanup from abandoned checkouts)
        var modelUuids = cartItems.Select(x => x.ModelUuid).ToList();
        await context.Orders
            .Where(x => x.UserUuid == userUuid
                        && modelUuids.Contains(x.ModelUuid)
                        && x.Status == nameof(OrderStatus.Pending))
            .ExecuteDeleteAsync();

        // 5. Calculate total amount
        var totalAmount = cartItems.Sum(x => x.ModelUu.Price);
        var amountInCents = (long)(totalAmount * 100);

        // 6. Get or create Stripe Customer
        var user = await context.Users.FirstAsync(x => x.Uuid == userUuid);
        var customerId = await GetOrCreateStripeCustomer(user);

        // 7. Create Ephemeral Key for Payment Sheet
        var ephemeralKey = await _ephemeralKeyService.CreateAsync(new EphemeralKeyCreateOptions
        {
            Customer = customerId,
            StripeVersion = "2025-01-27.acacia"
        });

        // 8. Create PaymentIntent
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

        // 9. Create Order for each model (status: pending)
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

        // 10. Return response for Flutter
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

        // 1. Find all Orders with this PaymentIntentId
        var orders = await context.Orders
            .Where(x => x.StripePaymentIntentId == paymentIntent.Id)
            .ToListAsync();

        if (orders.Count == 0) return;

        // Check if already processed (idempotency)
        if (orders.All(o => o.Status == nameof(OrderStatus.Completed)))
            return;

        var userUuid = orders.First().UserUuid;

        await using var transaction = await context.Database.BeginTransactionAsync();

        // 2. Update status to completed
        foreach (var order in orders)
        {
            order.Status = nameof(OrderStatus.Completed);
            order.UpdatedAt = DateTime.UtcNow;
        }

        // 3. Create Library entries
        foreach (var order in orders)
        {
            // Check if library entry already exists (idempotency)
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

        // 4. Delete ShoppingCart items
        var modelUuids = orders.Select(o => o.ModelUuid).ToList();
        await context.ShoppingCartItems
            .Where(x => x.UserUuid == userUuid && modelUuids.Contains(x.ModelUuid))
            .ExecuteDeleteAsync();

        await context.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    private async Task HandlePaymentFailed(Event stripeEvent)
    {
        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        if (paymentIntent == null) return;

        var orders = await context.Orders
            .Where(x => x.StripePaymentIntentId == paymentIntent.Id)
            .ToListAsync();

        foreach (var order in orders)
        {
            order.Status = nameof(OrderStatus.Failed);
            order.UpdatedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();
    }
}