using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostReviewType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPostReviewType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.CommunityPostReviewType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/community-post-review-types/")]
[ApiController]
public class CommunityPostReviewTypeController(ICommunityPostReviewTypeService service) : ControllerBase
{
    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("get/{communityPostReviewTypeUuid:guid}")]
    public async Task<ActionResult<CommunityPostReviewTypeResponse>> GetByUuid(
        [FromRoute] Guid communityPostReviewTypeUuid)
    {
        return await service.GetByUuid(communityPostReviewTypeUuid);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<CommunityPostReviewTypeResponse>>> Search(
        [FromQuery] CommunityPostReviewTypeSearchRequest request)
    {
        return await service.Search(request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpPost("create")]
    public async Task<ActionResult<CommunityPostReviewTypeResponse>> Create(
        [FromForm] CommunityPostReviewTypeCreateRequest request)
    {
        return await service.Create(request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpPatch("edit-community-post-review-type")]
    public async Task<ActionResult<CommunityPostReviewTypeResponse>> Patch(
        [FromForm] CommunityPostReviewTypePatchRequest request)
    {
        return await service.Patch(request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpDelete("delete/{communityPostReviewTypeUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid communityPostReviewTypeUuid)
    {
        await service.Delete(communityPostReviewTypeUuid);
        return NoContent();
    }
}