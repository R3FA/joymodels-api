using JoyModels.Models.DataTransferObjects.RequestTypes.ModelAvailability;
using JoyModels.Models.DataTransferObjects.ResponseTypes;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelAvailability;
using JoyModels.Services.Services.ModelAvailability;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/model-availability/")]
[ApiController]
public class ModelAvailabilityController(IModelAvailabilityService service) : ControllerBase
{
    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("get/{modelAvailabilityUuid:guid}")]
    public async Task<ActionResult<ModelAvailabilityResponse>> GetByUuid([FromRoute] Guid modelAvailabilityUuid)
    {
        return await service.GetByUuid(modelAvailabilityUuid);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<ModelAvailabilityResponse>>> Search(
        [FromQuery] ModelAvailabilitySearchRequest request)
    {
        return await service.Search(request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpPost("create")]
    public async Task<ActionResult<ModelAvailabilityResponse>> Create([FromBody] ModelAvailabilityCreateRequest request)
    {
        return await service.Create(request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpPatch("edit-model-availability/{modelAvailabilityUuid:guid}")]
    public async Task<ActionResult<ModelAvailabilityResponse>> Patch([FromRoute] Guid modelAvailabilityUuid,
        [FromBody] ModelAvailabilityPatchRequest request)
    {
        return await service.Patch(modelAvailabilityUuid, request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpDelete("delete/{modelAvailabilityUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid modelAvailabilityUuid)
    {
        await service.Delete(modelAvailabilityUuid);
        return NoContent();
    }
}