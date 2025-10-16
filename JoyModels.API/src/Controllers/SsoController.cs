using JoyModels.Models.DataTransferObjects.RequestTypes.Sso;
using JoyModels.Models.DataTransferObjects.ResponseTypes;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Sso;
using JoyModels.Services.Services.Sso;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[ApiController]
[Route("api/sso/")]
public class SsoController(ISsoService service) : ControllerBase
{
    [Authorize(Policy = "HeadStaff")]
    [HttpGet("get/{userUuid:guid}")]
    public async Task<ActionResult<SsoUserResponse>> GetByUuid([FromRoute] Guid userUuid)
    {
        return await service.GetByUuid(userUuid);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<SsoUserResponse>>> Search([FromQuery] SsoSearchRequest request)
    {
        return await service.Search(request);
    }

    [HttpPost("create")]
    public async Task<ActionResult<SsoUserResponse>> Create([FromBody] SsoUserCreateRequest request)
    {
        return await service.Create(request);
    }

    [Authorize(Policy = "UnverifiedUsers")]
    [HttpPost("verify/{userUuid:guid}")]
    public async Task<ActionResult<SsoUserResponse>> Verify([FromRoute] Guid userUuid,
        [FromBody] SsoVerifyRequest request)
    {
        return await service.Verify(userUuid, request);
    }

    [Authorize(Policy = "UnverifiedUsers")]
    [HttpPost("request-new-otp-code/{userUuid:guid}")]
    public async Task<ActionResult> RequestNewOtpCode([FromRoute] Guid userUuid,
        [FromBody] SsoNewOtpCodeRequest request)
    {
        await service.RequestNewOtpCode(userUuid, request);
        return Created();
    }

    [HttpPost("login")]
    public async Task<ActionResult<SsoLoginResponse>> Login([FromBody] SsoLoginRequest request)
    {
        return await service.Login(request);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpPost("request-access-token-change/{userUuid:guid}")]
    public async Task<ActionResult<SsoAccessTokenChangeResponse>> RequestAccessTokenChange(
        [FromRoute] Guid userUuid, [FromBody] SsoAccessTokenChangeRequest request)
    {
        return await service.RequestAccessTokenChange(userUuid, request);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpPost("logout/{userUuid:guid}")]
    public async Task<ActionResult> Logout([FromRoute] Guid userUuid,
        [FromBody] SsoLogoutRequest request)
    {
        await service.Logout(userUuid, request);
        return NoContent();
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpPatch("request-password-change/{userUuid:guid}")]
    public async Task<ActionResult> RequestPasswordChange([FromRoute] Guid userUuid,
        [FromBody] SsoPasswordChangeRequest request)
    {
        await service.RequestPasswordChange(userUuid, request);
        return NoContent();
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpPatch("set-role/{userUuid:guid}")]
    public async Task<ActionResult> SetRole([FromRoute] Guid userUuid,
        [FromBody] SsoSetRoleRequest request)
    {
        await service.SetRole(userUuid, request);
        return NoContent();
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpDelete("delete/{userUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid userUuid)
    {
        await service.Delete(userUuid);
        return NoContent();
    }
}