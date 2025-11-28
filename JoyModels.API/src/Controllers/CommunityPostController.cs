using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPost;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPost;
using JoyModels.Services.Services.CommunityPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/community-posts/")]
[ApiController]
public class CommunityPostController(ICommunityPostService service) : ControllerBase
{
    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("get/{communityPostUuid:guid}")]
    public async Task<ActionResult<CommunityPostResponse>> GetByUuid([FromRoute] Guid communityPostUuid)
    {
        return await service.GetByUuid(communityPostUuid);
    }

    // [Authorize(Policy = "VerifiedUsers")]
    // [HttpGet("search")]
    // public async Task<ActionResult<PaginationResponse<ModelResponse>>> Search([FromQuery] ModelSearchRequest request)
    // {
    //     return await service.Search(request);
    // }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpPost("create")]
    public async Task<ActionResult<CommunityPostResponse>> Create([FromForm] CommunityPostCreateRequest request)
    {
        return await service.Create(request);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpPost("create-user-review")]
    public async Task<ActionResult> CreateUserReview([FromForm] CommunityPostUserReviewCreateRequest request)
    {
        await service.CreateUserReview(request);
        return NoContent();
    }

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