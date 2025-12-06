using JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.ModelReviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/model-reviews/")]
[ApiController]
[Authorize(Policy = "VerifiedUsers")]
public class ModelReviewsController(IModelReviewService service) : ControllerBase
{
    [HttpGet("get/{modelReviewUuid:guid}")]
    public async Task<ActionResult<ModelReviewResponse>> GetByUuid([FromRoute] Guid modelReviewUuid)
    {
        return await service.GetByUuid(modelReviewUuid);
    }

    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<ModelReviewResponse>>> Search(
        [FromQuery] ModelReviewSearchRequest request)
    {
        return await service.Search(request);
    }

    [HttpPost("create")]
    public async Task<ActionResult<ModelReviewResponse>> Create([FromForm] ModelReviewCreateRequest request)
    {
        return await service.Create(request);
    }

    [HttpPatch("edit-model-review")]
    public async Task<ActionResult<ModelReviewResponse>> Patch([FromForm] ModelReviewPatchRequest request)
    {
        return await service.Patch(request);
    }

    [HttpDelete("delete/{modelReviewUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid modelReviewUuid)
    {
        await service.Delete(modelReviewUuid);
        return NoContent();
    }
}