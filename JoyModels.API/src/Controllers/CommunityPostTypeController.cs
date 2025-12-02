using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPostType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.CommunityPostType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/community-post-types/")]
[ApiController]
public class CommunityPostTypeController(ICommunityPostTypeService service) : ControllerBase
{
    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("get/{communityPostTypeUuid:guid}")]
    public async Task<ActionResult<CommunityPostTypeResponse>> GetByUuid([FromRoute] Guid communityPostTypeUuid)
    {
        return await service.GetByUuid(communityPostTypeUuid);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<CommunityPostTypeResponse>>> Search(
        [FromQuery] CommunityPostTypeSearchRequest request)
    {
        return await service.Search(request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpPost("create")]
    public async Task<ActionResult<CommunityPostTypeResponse>> Create([FromForm] CommunityPostTypeCreateRequest request)
    {
        return await service.Create(request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpPatch("edit-community-post-type")]
    public async Task<ActionResult<CommunityPostTypeResponse>> Patch([FromForm] CommunityPostTypePatchRequest request)
    {
        return await service.Patch(request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpDelete("delete/{communityPostTypeUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid communityPostTypeUuid)
    {
        await service.Delete(communityPostTypeUuid);
        return NoContent();
    }
}