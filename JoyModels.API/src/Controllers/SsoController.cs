using JoyModels.Models.DataTransferObjects.RequestTypes.Sso;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
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
    public async Task<ActionResult<SsoUserResponse>> Create([FromForm] SsoUserCreateRequest request)
    {
        return await service.Create(request);
    }

    [Authorize(Policy = "UnverifiedUsers")]
    [HttpPost("verify")]
    public async Task<ActionResult<SsoUserResponse>> Verify([FromForm] SsoVerifyRequest request)
    {
        return await service.Verify(request);
    }

    [Authorize(Policy = "UnverifiedUsers")]
    [HttpPost("request-new-otp-code")]
    public async Task<ActionResult> RequestNewOtpCode([FromForm] SsoNewOtpCodeRequest request)
    {
        await service.RequestNewOtpCode(request);
        return Created();
    }

    [HttpPost("login")]
    public async Task<ActionResult<SsoLoginResponse>> Login([FromForm] SsoLoginRequest request)
    {
        return await service.Login(request);
    }

    [HttpPost("admin-login")]
    public async Task<ActionResult<SsoLoginResponse>> AdminLogin([FromForm] SsoLoginRequest request)
    {
        return await service.AdminLogin(request);
    }

    [HttpPost("request-access-token-change")]
    public async Task<ActionResult<SsoAccessTokenChangeResponse>> RequestAccessTokenChange(
        [FromForm] SsoAccessTokenChangeRequest request)
    {
        return await service.RequestAccessTokenChange(request);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout([FromForm] SsoLogoutRequest request)
    {
        await service.Logout(request);
        return NoContent();
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpPatch("request-password-change")]
    public async Task<ActionResult> RequestPasswordChange([FromForm] SsoPasswordChangeRequest request)
    {
        await service.RequestPasswordChange(request);
        return NoContent();
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpPatch("set-role")]
    public async Task<ActionResult> SetRole([FromForm] SsoSetRoleRequest request)
    {
        await service.SetRole(request);
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