using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.ShoppingCart;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using JoyModels.Services.Validation;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.ShoppingCart.HelperMethods;

public static class ShoppingCartHelperMethods
{
    public static async Task<JoyModels.Models.Database.Entities.ShoppingCart> GetShoppingCartEntity(
        JoyModelsDbContext context, UserAuthValidation userAuthValidation, Guid modelUuid)
    {
        var shoppingCartItemEntity = await context.ShoppingCartItems
            .AsNoTracking()
            .Include(x => x.ModelUu)
            .ThenInclude(x => x.UserUu)
            .ThenInclude(x => x.UserRoleUu)
            .Include(x => x.ModelUu.ModelAvailabilityUu)
            .Include(x => x.ModelUu.ModelAvailabilityUu)
            .Include(x => x.ModelUu.ModelPictures)
            .FirstOrDefaultAsync(x =>
                x.ModelUuid == modelUuid && x.UserUuid == userAuthValidation.GetUserClaimUuid());

        return shoppingCartItemEntity ??
               throw new KeyNotFoundException($"Model with UUID `{modelUuid}` does not exist in shopping cart.");
    }

    public static async Task<PaginationBase<JoyModels.Models.Database.Entities.ShoppingCart>>
        SearchShoppingCartItemEntities(JoyModelsDbContext context, UserAuthValidation userAuthValidation,
            ShoppingCartSearchRequest request)
    {
        var baseQuery = context.ShoppingCartItems
            .AsNoTracking()
            .Include(x => x.ModelUu)
            .ThenInclude(x => x.UserUu)
            .ThenInclude(x => x.UserRoleUu)
            .Include(x => x.ModelUu.ModelAvailabilityUu)
            .Include(x => x.ModelUu.ModelAvailabilityUu)
            .Include(x => x.ModelUu.ModelPictures)
            .Where(x => x.UserUuid == userAuthValidation.GetUserClaimUuid());

        var filteredQuery = request.ModelName switch
        {
            not null => baseQuery.Where(x => x.ModelUu.Name.Contains(request.ModelName)),
            _ => baseQuery
        };

        filteredQuery =
            GlobalHelperMethods<JoyModels.Models.Database.Entities.ShoppingCart>.OrderBy(filteredQuery,
                request.OrderBy);

        var shoppingCartItemEntities = await PaginationBase<JoyModels.Models.Database.Entities.ShoppingCart>
            .CreateAsync(
                filteredQuery,
                request.PageNumber,
                request.PageSize,
                request.OrderBy);

        return shoppingCartItemEntities;
    }

    public static async Task CreateShoppingCartItemEntity(
        this JoyModels.Models.Database.Entities.ShoppingCart shoppingCartEntity, JoyModelsDbContext context)
    {
        await context.ShoppingCartItems.AddAsync(shoppingCartEntity);
        await context.SaveChangesAsync();
    }

    public static async Task DeleteShoppingCartItem(JoyModelsDbContext context, UserAuthValidation userAuthValidation,
        Guid modelUuid)
    {
        var totalRecords = await context.ShoppingCartItems
            .Where(x => x.ModelUuid == modelUuid && x.UserUuid == userAuthValidation.GetUserClaimUuid())
            .ExecuteDeleteAsync();

        if (totalRecords <= 0)
            throw new KeyNotFoundException(
                $"Model with UUID `{modelUuid}` does not exist in shopping cart.");

        await context.SaveChangesAsync();
    }
}