using JoyModels.Models.DataTransferObjects.Sso;
using JoyModels.Models.DataTransferObjects.Users;

namespace JoyModels.Services.Services.Sso;

public class SsoService : ISsoService
{
    public async Task<SsoGet> GetByUuid(string uuid)
    {
        throw new NotImplementedException();
    }

    public async Task<SsoGet> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<UserGet> Create(UserCreate user)
    {
        throw new NotImplementedException();
    }

    public async Task<UserGet> Verify()
    {
        throw new NotImplementedException();
    }

    public async Task Delete(string uuid)
    {
        throw new NotImplementedException();
    }
}