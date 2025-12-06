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
    [HttpGet("get/{shoppingCartItemUuid:guid}")]
    public async Task<ActionResult<ShoppingCartResponse>> GetByUuid([FromRoute] Guid shoppingCartItemUuid)
    {
        return await service.GetByUuid(shoppingCartItemUuid);
    }

    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<ShoppingCartResponse>>> Search(
        [FromQuery] ShoppingCartSearchRequest request)
    {
        return await service.Search(request);
    }

    [HttpPost("create")]
    public async Task<ActionResult<ShoppingCartResponse>> Create([FromBody] ShoppingCartCreateRequest request)
    {
        return await service.Create(request);
    }

    [HttpDelete("delete/{shoppingCartItemUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid shoppingCartItemUuid)
    {
        await service.Delete(shoppingCartItemUuid);
        return NoContent();
    }
}