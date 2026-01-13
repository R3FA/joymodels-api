using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.ShoppingCart;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ShoppingCart;
using JoyModels.Services.Services.ShoppingCart.HelperMethods;
using JoyModels.Services.Validation;

namespace JoyModels.Services.Services.ShoppingCart;

public class ShoppingCartService(JoyModelsDbContext context, IMapper mapper, UserAuthValidation userAuthValidation)
    : IShoppingCartService
{
    public async Task<ShoppingCartResponse> GetByUuid(Guid shoppingCartItemUuid)
    {
        var shoppingCartItemEntity =
            await ShoppingCartHelperMethods.GetShoppingCartEntity(context, userAuthValidation, shoppingCartItemUuid);
        return mapper.Map<ShoppingCartResponse>(shoppingCartItemEntity);
    }

    public async Task<PaginationResponse<ShoppingCartResponse>> Search(ShoppingCartSearchRequest request)
    {
        var shoppingCartItemEntities =
            await ShoppingCartHelperMethods.SearchShoppingCartItemEntities(context, userAuthValidation, request);

        return mapper.Map<PaginationResponse<ShoppingCartResponse>>(shoppingCartItemEntities);
    }

    public async Task<ShoppingCartResponse> Create(ShoppingCartCreateRequest request)
    {
        var shoppingCartItemEntity = mapper.Map<JoyModels.Models.Database.Entities.ShoppingCart>(request);
        shoppingCartItemEntity.UserUuid = userAuthValidation.GetUserClaimUuid();

        await shoppingCartItemEntity.CreateShoppingCartItemEntity(context);

        return await GetByUuid(shoppingCartItemEntity.Uuid);
    }

    public async Task Delete(Guid shoppingCartItemUuid)
    {
        await ShoppingCartHelperMethods.DeleteShoppingCartItem(context, userAuthValidation, shoppingCartItemUuid);
    }
}