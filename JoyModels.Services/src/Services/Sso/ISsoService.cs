using JoyModels.Models.DataTransferObjects.CustomResponseTypes;
using JoyModels.Models.DataTransferObjects.Sso;

namespace JoyModels.Services.Services.Sso;

public interface ISsoService
{
    Task<SsoReturn> GetByUuid(SsoGetByUuid request);
    Task<PaginationResponse<SsoReturn>> Search(SsoSearch request);
    Task<SsoUserGet> Create(SsoUserCreate request);
    Task<SsoUserGet> Verify(SsoVerify request);
    Task<SuccessResponse> RequestNewOtpCode(SsoRequestNewOtpCode request);
    Task<SsoLoginResponse> Login(SsoLogin request);
    Task<SuccessResponse> RequestPasswordChange(SsoRequestPasswordChange request);
    Task<SuccessResponse> Delete(SsoDelete request);
}