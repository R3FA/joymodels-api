using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/models/")]
[ApiController]
[Authorize(Policy = "VerifiedUsers")]
public class ModelController(IModelService service) : ControllerBase
{
    [HttpGet("get")]
    public async Task<ActionResult<ModelResponse>> GetByUuid([FromQuery] ModelGetByUuidRequest request)
    {
        return await service.GetByUuid(request);
    }

    [HttpGet("get/{modelUuid:guid}/images/{modelPictureLocationPath}")]
    public async Task<ActionResult> GetModelPictures([FromRoute] Guid modelUuid,
        [FromRoute] string modelPictureLocationPath)
    {
        var files = await service.GetModelPictures(modelUuid, modelPictureLocationPath);
        return File(files.FileBytes, files.ContentType);
    }

    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<ModelResponse>>> Search([FromQuery] ModelSearchRequest request)
    {
        return await service.Search(request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpGet("admin-search")]
    public async Task<ActionResult<PaginationResponse<ModelResponse>>> AdminSearch(
        [FromQuery] ModelAdminSearchRequest request)
    {
        return await service.AdminSearch(request);
    }

    [HttpGet("best-selling")]
    public async Task<ActionResult<PaginationResponse<ModelResponse>>> BestSelling(
        [FromQuery] ModelBestSellingRequest request)
    {
        return await service.BestSelling(request);
    }

    [HttpGet("recommended")]
    public async Task<ActionResult<PaginationResponse<ModelResponse>>> Recommended(
        [FromQuery] ModelRecommendedRequest request)
    {
        return await service.Recommended(request);
    }

    [HttpGet("is-model-liked/{modelUuid:guid}")]
    public async Task<ActionResult<bool>> IsModelLiked([FromRoute] Guid modelUuid)
    {
        return await service.IsModelLiked(modelUuid);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpPost("create")]
    public async Task<ActionResult<ModelResponse>> Create([FromForm] ModelCreateRequest request)
    {
        return await service.Create(request);
    }

    [HttpPost("model-like/{modelUuid:guid}")]
    public async Task<ActionResult> ModelLike([FromRoute] Guid modelUuid)
    {
        await service.ModelLike(modelUuid);
        return NoContent();
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpPatch("edit-model")]
    public async Task<ActionResult<ModelResponse>> Patch([FromForm] ModelPatchRequest request)
    {
        return await service.Patch(request);
    }

    [HttpDelete("model-unlike/{modelUuid:guid}")]
    public async Task<ActionResult> ModelUnlike([FromRoute] Guid modelUuid)
    {
        await service.ModelUnlike(modelUuid);
        return NoContent();
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpDelete("delete/{modelUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid modelUuid)
    {
        await service.Delete(modelUuid);
        return NoContent();
    }
}