using JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.ModelReviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/model-reviews/")]
[ApiController]
public class ModelReviewsController(IModelReviewService service) : ControllerBase
{
    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("get/{modelReviewUuid:guid}")]
    public async Task<ActionResult<ModelReviewResponse>> GetByUuid([FromRoute] Guid modelReviewUuid)
    {
        return await service.GetByUuid(modelReviewUuid);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<ModelReviewResponse>>> Search(
        [FromQuery] ModelReviewSearchRequest request)
    {
        return await service.Search(request);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpPost("create")]
    public async Task<ActionResult<ModelReviewResponse>> Create([FromForm] ModelReviewCreateRequest request)
    {
        return await service.Create(request);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpPatch("edit-model-review/{modelReviewUuid:guid}")]
    public async Task<ActionResult<ModelReviewResponse>> Patch([FromRoute] Guid modelReviewUuid,
        [FromForm] ModelReviewPatchRequest request)
    {
        return await service.Patch(modelReviewUuid, request);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpDelete("delete/{modelReviewUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid modelReviewUuid)
    {
        await service.Delete(modelReviewUuid);
        return NoContent();
    }
}