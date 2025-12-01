using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPost;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPost;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
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

    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<CommunityPostResponse>>> Search(
        [FromQuery] CommunityPostSearchRequest request)
    {
        return await service.Search(request);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("search-reviewed-users")]
    public async Task<ActionResult<PaginationResponse<CommunityPostUserReviewResponse>>> SearchReviewedUsers(
        [FromQuery] CommunityPostSearchReviewedUsersRequest request)
    {
        return await service.SearchReviewedUsers(request);
    }

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

    [Authorize(Policy = "VerifiedUsers")]
    [HttpPatch("edit-community-post")]
    public async Task<ActionResult<CommunityPostResponse>> Patch([FromForm] CommunityPostPatchRequest request)
    {
        return await service.Patch(request);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpDelete("delete-user-review")]
    public async Task<ActionResult> DeleteUserReview([FromForm] CommunityPostUserReviewDeleteRequest request)
    {
        await service.DeleteUserReview(request);
        return NoContent();
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpDelete("delete/{communityPostUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid communityPostUuid)
    {
        await service.Delete(communityPostUuid);
        return NoContent();
    }
}