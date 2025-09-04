using JoyModels.Models.DataTransferObjects.Sso;
using JoyModels.Models.DataTransferObjects.User;
using JoyModels.Models.DataTransferObjects.UserRole;
using JoyModels.Models.src.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.Sso;

public class SsoService : ISsoService
{
    private readonly JoyModelsDbContext _context;

    public SsoService(JoyModelsDbContext context)
    {
        _context = context;
    }

    public async Task<SsoGet> GetByUuid(string uuid)
    {
        if (!Guid.TryParse(uuid, out var guid))
            throw new ArgumentException($"UUID value `{uuid}` is invalid");

        var exists = await _context.PendingUsers.AsNoTracking().AnyAsync(x => x.Uuid.ToString() == uuid);
        if (!exists)
            throw new KeyNotFoundException($"There is no pending user with UUID value of `{uuid}`");

        var pendingUserEntity = await _context.PendingUsers
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .AsNoTracking()
            .FirstAsync(x => x.Uuid.ToString() == uuid);

        var pendingUser = new SsoGet()
        {
            Uuid = Guid.Parse(uuid),
            UserUuid = pendingUserEntity.UserUuid,
            User = new UserGet()
            {
                Uuid = pendingUserEntity.UserUuid,
                FirstName = pendingUserEntity.UserUu.FirstName,
                LastName = pendingUserEntity.UserUu.LastName,
                NickName = pendingUserEntity.UserUu.NickName,
                Email = pendingUserEntity.UserUu.Email,
                CreatedAt = pendingUserEntity.UserUu.CreatedAt,
                UserRoleUuid = pendingUserEntity.UserUu.UserRoleUuid,
                UserRole = new UserRoleGet()
                {
                    Uuid = pendingUserEntity.UserUu.UserRoleUu.Uuid,
                    RoleName = pendingUserEntity.UserUu.UserRoleUu.RoleName
                }
            }
        };

        return pendingUser;
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