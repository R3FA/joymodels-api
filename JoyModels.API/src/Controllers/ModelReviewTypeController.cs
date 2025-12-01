using JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviewsType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelReviewsType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.ModelReviewType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/model-review-type/")]
[ApiController]
public class ModelReviewTypeController(IModelReviewTypeService service) : ControllerBase
{
    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("get/{modelReviewTypeUuid:guid}")]
    public async Task<ActionResult<ModelReviewTypeResponse>> GetByUuid([FromRoute] Guid modelReviewTypeUuid)
    {
        return await service.GetByUuid(modelReviewTypeUuid);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<ModelReviewTypeResponse>>> Search(
        [FromQuery] ModelReviewTypeSearchRequest request)
    {
        return await service.Search(request);
    }

    // [Authorize(Policy = "HeadStaff")]
    // [HttpPost("create")]
    // public async Task<ActionResult<CategoryResponse>> Create([FromBody] CategoryCreateRequest request)
    // {
    //     return await service.Create(request);
    // }
    //
    // [Authorize(Policy = "HeadStaff")]
    // [HttpPatch("edit-category/{categoryUuid:guid}")]
    // public async Task<ActionResult<CategoryResponse>> Patch([FromRoute] Guid categoryUuid,
    //     [FromBody] CategoryPatchRequest request)
    // {
    //     return await service.Patch(categoryUuid, request);
    // }
    //
    // [Authorize(Policy = "HeadStaff")]
    // [HttpDelete("delete/{categoryUuid:guid}")]
    // public async Task<ActionResult> Delete([FromRoute] Guid categoryUuid)
    // {
    //     await service.Delete(categoryUuid);
    //     return NoContent();
    // }
}