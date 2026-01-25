using JoyModels.Models.DataTransferObjects.RequestTypes.Order;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Order;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;

namespace JoyModels.Services.Services.Orders;

public interface IOrderService
{
    Task<OrderCheckoutResponse> Checkout();
    Task<OrderConfirmResponse> Confirm(string paymentIntentId);
    Task<OrderResponse> GetByUuid(Guid orderUuid);
    Task<PaginationResponse<OrderResponse>> Search(OrderSearchRequest request);
    Task<PaginationResponse<OrderResponse>> AdminSearch(OrderAdminSearchRequest request);
}