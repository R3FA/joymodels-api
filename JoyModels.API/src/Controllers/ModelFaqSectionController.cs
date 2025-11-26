using JoyModels.Models.DataTransferObjects.RequestTypes.ModelFaqSection;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelFaqSection;
using JoyModels.Services.Services.ModelFaqSection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/model-faq-section/")]
[ApiController]
public class ModelFaqSectionController(IModelFaqSectionService service) : ControllerBase
{
    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("get/{modelFaqSectionUuid:guid}")]
    public async Task<ActionResult<ModelFaqSectionResponse>> GetByUuid([FromRoute] Guid modelFaqSectionUuid)
    {
        return await service.GetByUuid(modelFaqSectionUuid);
    }

    // [Authorize(Policy = "VerifiedUsers")]
    // [HttpGet("search")]
    // public async Task<ActionResult<PaginationResponse<ModelFaqSectionResponse>>> Search([FromQuery] ModelSearchRequest request)
    // {
    //     return await service.Search(request);
    // }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpPost("create")]
    public async Task<ActionResult<ModelFaqSectionResponse>> Create([FromForm] ModelFaqSectionCreateRequest request)
    {
        return await service.Create(request);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpPost("create-answer")]
    public async Task<ActionResult<ModelFaqSectionResponse>> CreateAnswer(
        [FromForm] ModelFaqSectionCreateAnswerRequest request)
    {
        return await service.CreateAnswer(request);
    }

    // [Authorize(Policy = "VerifiedUsers")]
    // [HttpPatch("edit-model/{modelUuid:guid}")]
    // public async Task<ActionResult<ModelFaqSectionResponse>> Patch([FromRoute] Guid modelUuid,
    //     [FromForm] ModelPatchRequest request)
    // {
    //     return await service.Patch(modelUuid, request);
    // }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpDelete("delete/{modelFaqSectionUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid modelFaqSectionUuid)
    {
        await service.Delete(modelFaqSectionUuid);
        return NoContent();
    }
}