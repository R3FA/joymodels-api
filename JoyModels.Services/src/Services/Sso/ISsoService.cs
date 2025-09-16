using JoyModels.Models.DataTransferObjects.CustomRequestTypes;
using JoyModels.Models.DataTransferObjects.Sso;
using JoyModels.Models.DataTransferObjects.User;

namespace JoyModels.Services.Services.Sso;

public interface ISsoService
{
    Task<SsoReturn> GetByUuid(SsoGet request);
    Task<SsoReturn> GetAll();
    Task<UserGet> Create(UserCreate user);
    Task<UserGet> Verify(SsoVerify request);
    Task<SuccessReturnDetails> ResendOtpCode(SsoResendOtpCode request);
    Task<SuccessReturnDetails> Delete(SsoDelete request);
}