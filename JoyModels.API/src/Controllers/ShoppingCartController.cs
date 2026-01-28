using JoyModels.Models.DataTransferObjects.RequestTypes.ShoppingCart;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ShoppingCart;
using JoyModels.Services.Services.ShoppingCart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/shopping-cart/")]
[ApiController]
[Authorize(Policy = "VerifiedUsers")]
public class ShoppingCartController(IShoppingCartService service) : ControllerBase
{
    [HttpGet("get/{modelUuid:guid}")]
    public async Task<ActionResult<ShoppingCartResponse>> GetByUuid([FromRoute] Guid modelUuid)
    {
        return await service.GetByUuid(modelUuid);
    }

    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<ShoppingCartResponse>>> Search(
        [FromQuery] ShoppingCartSearchRequest request)
    {
        return await service.Search(request);
    }

    [HttpPost("create")]
    public async Task<ActionResult<ShoppingCartResponse>> Create([FromForm] ShoppingCartCreateRequest request)
    {
        return await service.Create(request);
    }

    [HttpDelete("delete/{modelUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid modelUuid)
    {
        await service.Delete(modelUuid);
        return NoContent();
    }
}