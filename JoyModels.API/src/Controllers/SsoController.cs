using JoyModels.Models.DataTransferObjects.CustomResponseTypes;
using JoyModels.Models.DataTransferObjects.Sso;
using JoyModels.Services.Services.Sso;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SsoController : ControllerBase
{
    private readonly ISsoService _service;

    public SsoController(ISsoService service)
    {
        _service = service;
    }

    [HttpGet("GetByUuid")]
    public async Task<ActionResult<SsoReturn>> GetByUuid([FromQuery] SsoGetByUuid request)
    {
        return await _service.GetByUuid(request);
    }

    [HttpGet("Search")]
    public async Task<ActionResult<PaginationResponse<SsoReturn>>> Search([FromQuery] SsoSearch request)
    {
        return await _service.Search(request);
    }

    [HttpPost("Create")]
    public async Task<ActionResult<SsoUserGet>> Create([FromBody] SsoUserCreate request)
    {
        return await _service.Create(request);
    }

    [HttpPost("Verify")]
    public async Task<ActionResult<SsoUserGet>> Verify([FromBody] SsoVerify request)
    {
        return await _service.Verify(request);
    }

    [HttpPost("RequestNewOtpCode")]
    public async Task<ActionResult<SuccessResponse>> RequestNewOtpCode(
        [FromBody] SsoRequestNewOtpCode request)
    {
        return await _service.RequestNewOtpCode(request);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<SsoTokenResponse>> Login([FromBody] SsoLogin request)
    {
        return await _service.Login(request);
    }

    [HttpPatch("RequestPasswordChange")]
    public async Task<ActionResult<SuccessResponse>> RequestPasswordChange(
        [FromBody] SsoRequestPasswordChange request)
    {
        return await _service.RequestPasswordChange(request);
    }

    [HttpDelete("Delete")]
    public async Task<ActionResult<SuccessResponse>> Delete([FromQuery] SsoDelete request)
    {
        return await _service.Delete(request);
    }
}