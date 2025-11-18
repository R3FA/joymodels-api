using JoyModels.Models.DataTransferObjects.RequestTypes.Users;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;
using JoyModels.Services.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/users/")]
[ApiController]
public class UsersController(IUsersService service) : ControllerBase
{
    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("get/{userUuid:guid}")]
    public async Task<ActionResult<UsersResponse>> GetByUuid([FromRoute] Guid userUuid)
    {
        return await service.GetByUuid(userUuid);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<UsersResponse>>> Search([FromQuery] UsersSearchRequest request)
    {
        return await service.Search(request);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpPost("follow-an-user")]
    public async Task<ActionResult<UsersResponse>> FollowAnUser([FromQuery] UsersFollowRequest request)
    {
        return await service.FollowAnUser(request);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpPatch("edit-user/{userUuid:guid}")]
    public async Task<ActionResult<UsersResponse>> Patch([FromRoute] Guid userUuid,
        [FromForm] UsersPatchRequest request)
    {
        return await service.Patch(userUuid, request);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpDelete("delete/{userUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid userUuid)
    {
        await service.Delete(userUuid);
        return NoContent();
    }
}