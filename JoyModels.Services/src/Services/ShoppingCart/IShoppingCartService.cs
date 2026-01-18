using JoyModels.Models.DataTransferObjects.RequestTypes.ShoppingCart;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ShoppingCart;

namespace JoyModels.Services.Services.ShoppingCart;

public interface IShoppingCartService
{
    Task<ShoppingCartResponse> GetByUuid(Guid modelUuid);
    Task<PaginationResponse<ShoppingCartResponse>> Search(ShoppingCartSearchRequest request);
    Task<ShoppingCartResponse> Create(ShoppingCartCreateRequest request);
    Task Delete(Guid modelUuid);
}