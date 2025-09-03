using JoyModels.Models.DataTransferObjects.Sso;
using JoyModels.Models.DataTransferObjects.Users;

namespace JoyModels.Services.Services.Sso;

public interface ISsoService
{
    Task<SsoGet> GetByUuid(string uuid);
    Task<SsoGet> GetAll();
    Task<UserGet> Create(UserCreate user);
    Task<UserGet> Verify();
    Task Delete(string uuid);
}