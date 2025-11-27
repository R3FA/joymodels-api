using JoyModels.Services.Services.CommunityPost;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/community-posts/")]
[ApiController]
public class CommunityPostController(ICommunityPostService service) : ControllerBase
{
    // [Authorize(Policy = "VerifiedUsers")]
    // [HttpGet("get")]
    // public async Task<ActionResult<ModelResponse>> GetByUuid([FromQuery] ModelGetByUuidRequest request)
    // {
    //     return await service.GetByUuid(request);
    // }
    //
    // [Authorize(Policy = "VerifiedUsers")]
    // [HttpGet("search")]
    // public async Task<ActionResult<PaginationResponse<ModelResponse>>> Search([FromQuery] ModelSearchRequest request)
    // {
    //     return await service.Search(request);
    // }
    //
    // [Authorize(Policy = "HeadStaff")]
    // [HttpGet("admin-search")]
    // public async Task<ActionResult<PaginationResponse<ModelResponse>>> AdminSearch(
    //     [FromQuery] ModelAdminSearchRequest request)
    // {
    //     return await service.AdminSearch(request);
    // }
    //
    // [Authorize(Policy = "VerifiedUsers")]
    // [HttpPost("create")]
    // public async Task<ActionResult<ModelResponse>> Create([FromForm] ModelCreateRequest request)
    // {
    //     return await service.Create(request);
    // }
    //
    // [Authorize(Policy = "VerifiedUsers")]
    // [HttpPost("model-like/{modelUuid:guid}")]
    // public async Task<ActionResult> ModelLike([FromRoute] Guid modelUuid)
    // {
    //     await service.ModelLike(modelUuid);
    //     return NoContent();
    // }
    //
    // [Authorize(Policy = "VerifiedUsers")]
    // [HttpPatch("edit-model/{modelUuid:guid}")]
    // public async Task<ActionResult<ModelResponse>> Patch([FromRoute] Guid modelUuid,
    //     [FromForm] ModelPatchRequest request)
    // {
    //     return await service.Patch(modelUuid, request);
    // }
    //
    // [Authorize(Policy = "VerifiedUsers")]
    // [HttpDelete("model-unlike/{modelUuid:guid}")]
    // public async Task<ActionResult> ModelUnlike([FromRoute] Guid modelUuid)
    // {
    //     await service.ModelUnlike(modelUuid);
    //     return NoContent();
    // }
    //
    // [Authorize(Policy = "VerifiedUsers")]
    // [HttpDelete("delete/{modelUuid:guid}")]
    // public async Task<ActionResult> Delete([FromRoute] Guid modelUuid)
    // {
    //     await service.Delete(modelUuid);
    //     return NoContent();
    // }
}