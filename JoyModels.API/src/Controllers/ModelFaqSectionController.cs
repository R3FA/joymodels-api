using JoyModels.Models.DataTransferObjects.RequestTypes.ModelFaqSection;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelFaqSection;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.ModelFaqSection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/model-faq-section/")]
[ApiController]
[Authorize(Policy = "VerifiedUsers")]
public class ModelFaqSectionController(IModelFaqSectionService service) : ControllerBase
{
    [HttpGet("get/{modelFaqSectionUuid:guid}")]
    public async Task<ActionResult<ModelFaqSectionResponse>> GetByUuid([FromRoute] Guid modelFaqSectionUuid)
    {
        return await service.GetByUuid(modelFaqSectionUuid);
    }

    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<ModelFaqSectionResponse>>> Search(
        [FromQuery] ModelFaqSectionSearchRequest request)
    {
        return await service.Search(request);
    }

    [HttpPost("create")]
    public async Task<ActionResult<ModelFaqSectionResponse>> Create([FromForm] ModelFaqSectionCreateRequest request)
    {
        return await service.Create(request);
    }

    [HttpPost("create-answer")]
    public async Task<ActionResult<ModelFaqSectionResponse>> CreateAnswer(
        [FromForm] ModelFaqSectionCreateAnswerRequest request)
    {
        return await service.CreateAnswer(request);
    }

    [HttpPatch("patch")]
    public async Task<ActionResult<ModelFaqSectionResponse>> Patch([FromForm] ModelFaqSectionPatchRequest request)
    {
        return await service.Patch(request);
    }

    [HttpDelete("delete")]
    public async Task<ActionResult> Delete([FromRoute] ModelFaqSectionDeleteRequest request)
    {
        await service.Delete(request);
        return NoContent();
    }
}