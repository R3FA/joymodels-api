using JoyModels.Models.DataTransferObjects.RequestTypes.Categories;
using JoyModels.Models.DataTransferObjects.ResponseTypes;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Categories;
using JoyModels.Services.Services.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/categories/")]
[ApiController]
public class CategoryController(ICategoryService service) : ControllerBase
{
    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("get/{categoryUuid:guid}")]
    public async Task<ActionResult<CategoryResponse>> GetByUuid([FromRoute] Guid categoryUuid)
    {
        return await service.GetByUuid(categoryUuid);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<CategoryResponse>>> Search(
        [FromQuery] CategorySearchRequest request)
    {
        return await service.Search(request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpPost("create")]
    public async Task<ActionResult<CategoryResponse>> Create([FromBody] CategoryCreateRequest request)
    {
        return await service.Create(request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpPatch("edit-category/{categoryUuid:guid}")]
    public async Task<ActionResult<CategoryResponse>> Patch([FromRoute] Guid categoryUuid,
        [FromBody] CategoryPatchRequest request)
    {
        return await service.Patch(categoryUuid, request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpDelete("delete/{categoryUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid categoryUuid)
    {
        await service.Delete(categoryUuid);
        return NoContent();
    }
}