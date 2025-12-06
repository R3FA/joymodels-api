using JoyModels.Models.DataTransferObjects.RequestTypes.Users;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;
using JoyModels.Services.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/users/")]
[ApiController]
[Authorize(Policy = "VerifiedUsers")]
public class UsersController(IUsersService service) : ControllerBase
{
    [HttpGet("get/{userUuid:guid}")]
    public async Task<ActionResult<UsersResponse>> GetByUuid([FromRoute] Guid userUuid)
    {
        return await service.GetByUuid(userUuid);
    }

    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<UsersResponse>>> Search([FromQuery] UsersSearchRequest request)
    {
        return await service.Search(request);
    }

    [HttpGet("search-following-users")]
    public async Task<ActionResult<PaginationResponse<UserFollowingResponse>>> SearchFollowingUsers(
        [FromQuery] UserFollowerSearchRequest request)
    {
        return await service.SearchFollowingUsers(request);
    }

    [HttpGet("search-follower-users")]
    public async Task<ActionResult<PaginationResponse<UserFollowerResponse>>> SearchFollowerUsers(
        [FromQuery] UserFollowerSearchRequest request)
    {
        return await service.SearchFollowerUsers(request);
    }

    [HttpGet("search-user-model-likes")]
    public async Task<ActionResult<PaginationResponse<UserModelLikesSearchResponse>>> SearchUserModelLikes(
        [FromQuery] UserModelLikesSearchRequest request)
    {
        return await service.SearchUserModelLikes(request);
    }

    [HttpPost("follow-an-user/{targetUserUuid:guid}")]
    public async Task<ActionResult> FollowAnUser([FromRoute] Guid targetUserUuid)
    {
        await service.FollowAnUser(targetUserUuid);
        return NoContent();
    }

    [HttpPatch("edit-user")]
    public async Task<ActionResult<UsersResponse>> Patch([FromForm] UsersPatchRequest request)
    {
        return await service.Patch(request);
    }

    [HttpDelete("unfollow-an-user/{targetUserUuid:guid}")]
    public async Task<ActionResult> UnfollowAnUser([FromRoute] Guid targetUserUuid)
    {
        await service.UnfollowAnUser(targetUserUuid);
        return NoContent();
    }

    [HttpDelete("delete/{userUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid userUuid)
    {
        await service.Delete(userUuid);
        return NoContent();
    }
}