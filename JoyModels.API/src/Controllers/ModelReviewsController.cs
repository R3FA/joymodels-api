using JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelReviews;
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
    [HttpPost("create")]
    public async Task<ActionResult<ModelReviewResponse>> Create([FromForm] ModelReviewCreateRequest request)
    {
        return await service.Create(request);
    }
}