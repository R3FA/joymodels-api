using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;
using JoyModels.Services.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/models/")]
[ApiController]
public class ModelController(IModelService service) : ControllerBase
{
    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("get/{modelUuid:guid}")]
    public async Task<ActionResult<ModelResponse>> GetByUuid([FromRoute] Guid modelUuid)
    {
        return await service.GetByUuid(modelUuid);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<ModelResponse>>> Search([FromQuery] ModelSearchRequest request)
    {
        return await service.Search(request);
    }

    [NonAction]
    [Authorize(Policy = "VerifiedUsers")]
    [HttpPost("create")]
    public async Task<ActionResult<ModelResponse>> Create([FromBody] ModelCreateRequest request)
    {
        return await service.Create(request);
    }
}