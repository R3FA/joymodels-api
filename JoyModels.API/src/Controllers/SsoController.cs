using JoyModels.Models.DataTransferObjects.RequestTypes.Sso;
using JoyModels.Models.DataTransferObjects.ResponseTypes;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Sso;
using JoyModels.Services.Services.Sso;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Policy = "HeadStaff")]
    [HttpGet("Get/{userUuid:guid}")]
    public async Task<ActionResult<SsoResponse>> GetByUuid([FromRoute] Guid userUuid)
    {
        return await _service.GetByUuid(userUuid);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpGet("Search")]
    public async Task<ActionResult<PaginationResponse<SsoResponse>>> Search([FromQuery] SsoSearchRequest request)
    {
        return await _service.Search(request);
    }

    [HttpPost("Create")]
    public async Task<ActionResult<SsoUserResponse>> Create([FromBody] SsoUserCreateRequest request)
    {
        return await _service.Create(request);
    }

    [Authorize(Policy = "UnverifiedUsers")]
    [HttpPost("Verify/{userUuid:guid}")]
    public async Task<ActionResult<SsoUserResponse>> Verify([FromRoute] Guid userUuid,
        [FromBody] SsoVerifyRequest request)
    {
        return await _service.Verify(userUuid, request);
    }

    [Authorize(Policy = "UnverifiedUsers")]
    [HttpPost("RequestNewOtpCode/{userUuid:guid}")]
    public async Task<ActionResult<SuccessResponse>> RequestNewOtpCode([FromRoute] Guid userUuid,
        [FromBody] SsoNewOtpCodeRequest request)
    {
        return await _service.RequestNewOtpCode(userUuid, request);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<SsoLoginResponse>> Login([FromBody] SsoLoginRequest request)
    {
        return await _service.Login(request);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpPost("RequestAccessTokenChange/{userUuid:guid}")]
    public async Task<ActionResult<SsoAccessTokenChangeResponse>> RequestAccessTokenChange(
        [FromRoute] Guid userUuid, [FromBody] SsoAccessTokenChangeRequest request)
    {
        return await _service.RequestAccessTokenChange(userUuid, request);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpPost("Logout/{userUuid:guid}")]
    public async Task<ActionResult<SuccessResponse>> Logout([FromRoute] Guid userUuid,
        [FromBody] SsoLogoutRequest request)
    {
        return await _service.Logout(userUuid, request);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpPatch("RequestPasswordChange/{userUuid:guid}")]
    public async Task<ActionResult<SuccessResponse>> RequestPasswordChange([FromRoute] Guid userUuid,
        [FromBody] SsoPasswordChangeRequest request)
    {
        return await _service.RequestPasswordChange(userUuid, request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpPatch("SetRole/{userUuid:guid}")]
    public async Task<ActionResult<SuccessResponse>> SetRole([FromRoute] Guid userUuid,
        [FromBody] SsoSetRoleRequest request)
    {
        return await _service.SetRole(userUuid, request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpDelete("Delete/{userUuid:guid}")]
    public async Task<ActionResult<SuccessResponse>> Delete([FromRoute] Guid userUuid)
    {
        return await _service.Delete(userUuid);
    }
}