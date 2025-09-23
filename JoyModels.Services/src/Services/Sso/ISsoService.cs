using JoyModels.Models.DataTransferObjects.RequestTypes.Sso;
using JoyModels.Models.DataTransferObjects.ResponseTypes;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Sso;

namespace JoyModels.Services.Services.Sso;

public interface ISsoService
{
    Task<SsoResponse> GetByUuid(Guid userUuid);
    Task<PaginationResponse<SsoResponse>> Search(SsoSearchRequest request);
    Task<SsoUserResponse> Create(SsoUserCreateRequest request);
    Task<SsoUserResponse> Verify(Guid userUuid, SsoVerifyRequest request);
    Task<SuccessResponse> RequestNewOtpCode(Guid userUuid, SsoNewOtpCodeRequest newOtpCodeRequest);
    Task<SsoLoginResponse> Login(SsoLoginRequest request);

    Task<SsoAccessTokenChangeResponse> RequestAccessTokenChange(Guid userUuid,
        SsoAccessTokenChangeRequest accessTokenChangeRequest);

    Task<SuccessResponse> Logout(Guid userUuid, SsoLogoutRequest request);
    Task<SuccessResponse> RequestPasswordChange(Guid userUuid, SsoPasswordChangeRequest passwordChangeRequest);
    Task<SuccessResponse> Delete(Guid userUuid);
}