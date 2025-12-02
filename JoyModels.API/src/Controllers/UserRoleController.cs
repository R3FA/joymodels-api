using JoyModels.Models.DataTransferObjects.RequestTypes.UserRole;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.UserRole;
using JoyModels.Services.Services.UserRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/user-roles/")]
[ApiController]
[Authorize(Policy = "HeadStaff")]
public class UserRoleController(IUserRoleService service) : ControllerBase
{
    [HttpGet("get/{userRoleUuid:guid}")]
    public async Task<ActionResult<UserRoleResponse>> GetByUuid([FromRoute] Guid userRoleUuid)
    {
        return await service.GetByUuid(userRoleUuid);
    }

    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<UserRoleResponse>>> Search(
        [FromQuery] UserRoleSearchRequest request)
    {
        return await service.Search(request);
    }

    [HttpPost("create")]
    public async Task<ActionResult<UserRoleResponse>> Create([FromForm] UserRoleCreateRequest request)
    {
        return await service.Create(request);
    }

    [HttpPatch("edit-user-role")]
    public async Task<ActionResult<UserRoleResponse>> Patch([FromForm] UserRolePatchRequest request)
    {
        return await service.Patch(request);
    }

    [HttpDelete("delete/{userRoleUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid userRoleUuid)
    {
        await service.Delete(userRoleUuid);
        return NoContent();
    }
}