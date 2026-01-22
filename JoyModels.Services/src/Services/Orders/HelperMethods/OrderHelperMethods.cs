using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.Order;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using JoyModels.Services.Validation;
using Microsoft.EntityFrameworkCore;
using ShoppingCartEntity = JoyModels.Models.Database.Entities.ShoppingCart;

namespace JoyModels.Services.Services.Orders.HelperMethods;

public static class OrderHelperMethods
{
    public static async Task<List<ShoppingCartEntity>> GetUserCartItemsWithModels(JoyModelsDbContext context,
        Guid userUuid)
    {
        return await context.ShoppingCartItems
            .AsNoTracking()
            .Include(x => x.ModelUu)
            .Where(x => x.UserUuid == userUuid)
            .ToListAsync();
    }

    public static async Task ValidateModelsNotAlreadyOwned(
        JoyModelsDbContext context,
        Guid userUuid,
        List<ShoppingCartEntity> cartItems)
    {
        var modelUuids = cartItems.Select(x => x.ModelUuid).ToList();

        var ownedModels = await context.Libraries
            .AsNoTracking()
            .Where(x => x.UserUuid == userUuid && modelUuids.Contains(x.ModelUuid))
            .Select(x => x.Model.Name)
            .ToListAsync();

        if (ownedModels.Count > 0)
            throw new ArgumentException($"You already own these models: {string.Join(", ", ownedModels)}");
    }

    public static async Task<Order> GetOrderEntity(JoyModelsDbContext context, Guid userUuid, Guid orderUuid)
    {
        var orderEntity = await context.Orders
            .AsNoTracking()
            .Include(x => x.Model)
            .ThenInclude(x => x.UserUu)
            .ThenInclude(x => x.UserRoleUu)
            .Include(x => x.Model.ModelAvailabilityUu)
            .Include(x => x.Model.ModelPictures)
            .FirstOrDefaultAsync(x => x.Uuid == orderUuid && x.UserUuid == userUuid);

        return orderEntity ??
               throw new KeyNotFoundException($"Order with UUID `{orderUuid}` does not exist.");
    }

    public static async Task<PaginationBase<Order>> SearchOrderEntities(
        JoyModelsDbContext context,
        UserAuthValidation userAuthValidation,
        OrderSearchRequest request)
    {
        var userUuid = userAuthValidation.GetUserClaimUuid();

        var baseQuery = context.Orders
            .AsNoTracking()
            .Include(x => x.Model)
            .ThenInclude(x => x.UserUu)
            .ThenInclude(x => x.UserRoleUu)
            .Include(x => x.Model.ModelAvailabilityUu)
            .Include(x => x.Model.ModelPictures)
            .Where(x => x.UserUuid == userUuid);

        var filteredQuery = request.Status switch
        {
            not null => baseQuery.Where(x => x.Status == request.Status),
            _ => baseQuery
        };

        filteredQuery = GlobalHelperMethods<Order>.OrderBy(filteredQuery, request.OrderBy);

        return await PaginationBase<Order>.CreateAsync(
            filteredQuery,
            request.PageNumber,
            request.PageSize,
            request.OrderBy);
    }

    public static async Task<PaginationBase<Order>> AdminSearchOrderEntities(
        JoyModelsDbContext context,
        OrderAdminSearchRequest request)
    {
        var baseQuery = context.Orders
            .AsNoTracking()
            .Include(x => x.Model)
            .ThenInclude(x => x.UserUu)
            .ThenInclude(x => x.UserRoleUu)
            .Include(x => x.Model.ModelAvailabilityUu)
            .Include(x => x.Model.ModelPictures)
            .AsQueryable();

        if (request.UserUuid.HasValue)
            baseQuery = baseQuery.Where(x => x.UserUuid == request.UserUuid.Value);

        if (!string.IsNullOrEmpty(request.Status))
            baseQuery = baseQuery.Where(x => x.Status == request.Status);

        if (!string.IsNullOrEmpty(request.StripePaymentIntentId))
            baseQuery = baseQuery.Where(x => x.StripePaymentIntentId == request.StripePaymentIntentId);

        baseQuery = GlobalHelperMethods<Order>.OrderBy(baseQuery, request.OrderBy);

        return await PaginationBase<Order>.CreateAsync(
            baseQuery,
            request.PageNumber,
            request.PageSize,
            request.OrderBy);
    }
}