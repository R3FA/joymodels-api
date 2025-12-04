using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostQuestionSection;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPostQuestionSection;
using JoyModels.Services.Services.CommunityPostQuestionSection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/community-post-question-section/")]
[ApiController]
[Authorize(Policy = "VerifiedUsers")]
public class CommunityPostQuestionSectionController(ICommunityPostQuestionSectionService service) : ControllerBase
{
    [HttpGet("get/{communityPostQuestionSectionUuid:guid}")]
    public async Task<ActionResult<CommunityPostQuestionSectionResponse>> GetByUuid(
        [FromRoute] Guid communityPostQuestionSectionUuid)
    {
        return await service.GetByUuid(communityPostQuestionSectionUuid);
    }

    [HttpPost("create")]
    public async Task<ActionResult<CommunityPostQuestionSectionResponse>> Create(
        [FromForm] CommunityPostQuestionSectionCreateRequest request)
    {
        return await service.Create(request);
    }

    [HttpPost("create-answer")]
    public async Task<ActionResult<CommunityPostQuestionSectionResponse>> CreateAnswer(
        [FromForm] CommunityPostQuestionSectionCreateAnswerRequest request)
    {
        return await service.CreateAnswer(request);
    }

    [HttpPatch("patch")]
    public async Task<ActionResult<CommunityPostQuestionSectionResponse>> Patch(
        [FromForm] CommunityPostQuestionSectionPatchRequest request)
    {
        return await service.Patch(request);
    }

    [HttpDelete("delete/{communityPostQuestionSectionUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid communityPostQuestionSectionUuid)
    {
        await service.Delete(communityPostQuestionSectionUuid);
        return NoContent();
    }
}