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

    [Authorize(Policy = "HeadStaff")]
    [HttpPost("create")]
    public async Task<ActionResult<ModelReviewTypeResponse>> Create([FromForm] ModelReviewTypeCreateRequest request)
    {
        return await service.Create(request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpPatch("edit-model-review-type")]
    public async Task<ActionResult<ModelReviewTypeResponse>> Patch([FromForm] ModelReviewTypePatchRequest request)
    {
        return await service.Patch(request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpDelete("delete/{modelReviewTypeUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid modelReviewTypeUuid)
    {
        await service.Delete(modelReviewTypeUuid);
        return NoContent();
    }
}