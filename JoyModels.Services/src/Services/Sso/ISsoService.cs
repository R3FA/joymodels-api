using JoyModels.Models.DataTransferObjects.RequestTypes.Sso;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Sso;

namespace JoyModels.Services.Services.Sso;

public interface ISsoService
{
    Task<SsoUserResponse> GetByUuid(Guid userUuid);
    Task<PaginationResponse<SsoUserResponse>> Search(SsoSearchRequest request);
    Task<SsoUserResponse> Create(SsoUserCreateRequest request);
    Task<SsoUserResponse> Verify(SsoVerifyRequest request);
    Task RequestNewOtpCode(SsoNewOtpCodeRequest request);
    Task<SsoLoginResponse> Login(SsoLoginRequest request);
    Task<SsoAccessTokenChangeResponse> RequestAccessTokenChange(SsoAccessTokenChangeRequest request);
    Task Logout(SsoLogoutRequest request);
    Task RequestPasswordChange(SsoPasswordChangeRequest request);
    Task SetRole(SsoSetRoleRequest request);
    Task Delete(Guid userUuid);
}