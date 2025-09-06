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

        var pendingUserEntity = await _context.PendingUsers
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Uuid.ToString() == uuid || x.UserUuid.ToString() == uuid);

        if (pendingUserEntity == null)
            throw new KeyNotFoundException($"Pending user with uuid `{uuid}` not found");

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
        user.ValidateUserCreation();
        var hashedPassword = user.GeneratePasswordHash(user.Password);

        return new UserGet();
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