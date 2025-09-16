using JoyModels.Models.DataTransferObjects.CustomReturnTypes;
using JoyModels.Models.DataTransferObjects.Sso;

namespace JoyModels.Services.Services.Sso;

public interface ISsoService
{
    Task<SsoReturn> GetByUuid(SsoGetByUuid request);
    Task<PaginationResponse<SsoReturn>> Search(SsoSearch request);
    Task<SsoUserGet> Create(SsoUserCreate request);
    Task<SsoUserGet> Verify(SsoVerify request);
    Task<SuccessReturnDetails> ResendOtpCode(SsoResendOtpCode request);
    Task<SuccessReturnDetails> Delete(SsoDelete request);
}