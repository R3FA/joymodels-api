using JoyModels.Models.DataTransferObjects.RequestTypes.Order;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Order;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/orders/")]
[ApiController]
[Authorize(Policy = "VerifiedUsers")]
public class OrderController(IOrderService service) : ControllerBase
{
    [HttpPost("checkout")]
    public async Task<ActionResult<OrderCheckoutResponse>> Checkout()
    {
        return await service.Checkout();
    }

    [HttpPost("confirm/{paymentIntentId}")]
    public async Task<ActionResult<OrderConfirmResponse>> Confirm([FromRoute] string paymentIntentId)
    {
        var result = await service.Confirm(paymentIntentId);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("get/{orderUuid:guid}")]
    public async Task<ActionResult<OrderResponse>> GetByUuid([FromRoute] Guid orderUuid)
    {
        return await service.GetByUuid(orderUuid);
    }

    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<OrderResponse>>> Search(
        [FromQuery] OrderSearchRequest request)
    {
        return await service.Search(request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpGet("admin-search")]
    public async Task<ActionResult<PaginationResponse<OrderResponse>>> AdminSearch(
        [FromQuery] OrderAdminSearchRequest request)
    {
        return await service.AdminSearch(request);
    }
}