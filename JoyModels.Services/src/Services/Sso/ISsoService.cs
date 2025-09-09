using JoyModels.Models.DataTransferObjects.Sso;
using JoyModels.Models.DataTransferObjects.User;

namespace JoyModels.Services.Services.Sso;

public interface ISsoService
{
    Task<SsoReturn> GetByUuid(SsoGet request);
    Task<SsoReturn> GetAll();
    Task<UserGet> Create(UserCreate user);
    Task<UserGet> Verify(SsoVerify request);
    Task Delete(string uuid);
}