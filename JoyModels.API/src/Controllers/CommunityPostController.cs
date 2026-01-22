using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPost;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPost;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.CommunityPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/community-posts/")]
[ApiController]
[Authorize(Policy = "VerifiedUsers")]
public class CommunityPostController(ICommunityPostService service) : ControllerBase
{
    [HttpGet("get/{communityPostUuid:guid}")]
    public async Task<ActionResult<CommunityPostResponse>> GetByUuid([FromRoute] Guid communityPostUuid)
    {
        return await service.GetByUuid(communityPostUuid);
    }

    [HttpGet("get/{communityPostUuid:guid}/images/{communityPostPictureLocationPath}")]
    public async Task<ActionResult> GetCommunityPostPictures([FromRoute] Guid communityPostUuid,
        [FromRoute] string communityPostPictureLocationPath)
    {
        var files = await service.GetCommunityPostPictures(communityPostUuid, communityPostPictureLocationPath);
        return File(files.FileBytes, files.ContentType);
    }

    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<CommunityPostResponse>>> Search(
        [FromQuery] CommunityPostSearchRequest request)
    {
        return await service.Search(request);
    }

    [HttpGet("search-reviewed-users")]
    public async Task<ActionResult<PaginationResponse<CommunityPostUserReviewResponse>>> SearchReviewedUsers(
        [FromQuery] CommunityPostSearchReviewedUsersRequest request)
    {
        return await service.SearchReviewedUsers(request);
    }

    [HttpGet("search-users-liked-posts")]
    public async Task<ActionResult<PaginationResponse<CommunityPostResponse>>> SearchUsersLikedPosts(
        [FromQuery] CommunityPostSearchUserLikedPosts request)
    {
        return await service.SearchUsersLikedPosts(request);
    }

    [HttpPost("create")]
    public async Task<ActionResult<CommunityPostResponse>> Create([FromForm] CommunityPostCreateRequest request)
    {
        return await service.Create(request);
    }

    [HttpGet("is-liked/{communityPostUuid:guid}")]
    public async Task<ActionResult<bool>> IsLiked([FromRoute] Guid communityPostUuid)
    {
        return await service.IsLiked(communityPostUuid);
    }

    [HttpPost("create-user-review")]
    public async Task<ActionResult> CreateUserReview([FromForm] CommunityPostUserReviewCreateRequest request)
    {
        await service.CreateUserReview(request);
        return NoContent();
    }

    [HttpPatch("edit-community-post")]
    public async Task<ActionResult<CommunityPostResponse>> Patch([FromForm] CommunityPostPatchRequest request)
    {
        return await service.Patch(request);
    }

    [HttpGet("is-disliked/{communityPostUuid:guid}")]
    public async Task<ActionResult<bool>> IsDisliked([FromRoute] Guid communityPostUuid)
    {
        return await service.IsDisliked(communityPostUuid);
    }

    [HttpDelete("delete-user-review")]
    public async Task<ActionResult> DeleteUserReview([FromForm] CommunityPostUserReviewDeleteRequest request)
    {
        await service.DeleteUserReview(request);
        return NoContent();
    }

    [HttpDelete("delete/{communityPostUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid communityPostUuid)
    {
        await service.Delete(communityPostUuid);
        return NoContent();
    }
}