using JoyModels.Models.DataTransferObjects.RequestTypes.Order;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Order;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/orders/")]
[ApiController]
public class OrderController(IOrderService service) : ControllerBase
{
    [Authorize(Policy = "VerifiedUsers")]
    [HttpPost("checkout")]
    public async Task<ActionResult<OrderCheckoutResponse>> Checkout()
    {
        return await service.Checkout();
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpPost("confirm/{paymentIntentId}")]
    public async Task<ActionResult<OrderConfirmResponse>> Confirm([FromRoute] string paymentIntentId)
    {
        var result = await service.Confirm(paymentIntentId);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"];

        if (string.IsNullOrEmpty(signature))
            return BadRequest("Missing Stripe-Signature header");

        await service.HandleWebhook(json, signature!);
        return Ok();
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("get/{orderUuid:guid}")]
    public async Task<ActionResult<OrderResponse>> GetByUuid([FromRoute] Guid orderUuid)
    {
        return await service.GetByUuid(orderUuid);
    }

    [Authorize(Policy = "VerifiedUsers")]
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