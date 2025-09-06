using System.Transactions;
using JoyModels.Models.DataTransferObjects.Sso;
using JoyModels.Models.DataTransferObjects.User;
using JoyModels.Models.DataTransferObjects.UserRole;
using JoyModels.Models.src.Database.Entities;
using Microsoft.EntityFrameworkCore;
using UserRole = JoyModels.Models.Enums.UserRole;

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
            .FirstAsync(x => x.Uuid.ToString() == uuid || x.UserUuid.ToString() == uuid);

        if (pendingUserEntity == null)
            throw new KeyNotFoundException($"Pending user with uuid `{uuid}` not found");

        var pendingUser = new SsoGet()
        {
            Uuid = pendingUserEntity.Uuid,
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

        var userRoleEntity = await _context.UserRoles
            .AsNoTracking()
            .FirstAsync(x => x.RoleName == nameof(UserRole.Unverified));

        var userEntity = new User()
        {
            Uuid = Guid.NewGuid(),
            FirstName = user.FirstName,
            LastName = user.LastName,
            NickName = user.Nickname,
            Email = user.Email,
            PasswordHash = user.GeneratePasswordHash(user.Password),
            CreatedAt = DateTime.Now,
            UserRoleUuid = userRoleEntity.Uuid
        };

        var pendingUserEntity = new PendingUser()
        {
            Uuid = Guid.NewGuid(),
            UserUuid = userEntity.Uuid,
            OtpCode = SsoHelperMethods.GenerateOtpCode(),
            OtpCreatedAt = DateTime.Now,
            OtpExpirationDate = DateTime.Now.AddMinutes(60)
        };

        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Users.Add(userEntity);
            await _context.SaveChangesAsync();

            _context.PendingUsers.Add(pendingUserEntity);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            throw new TransactionException(ex.Message, ex);
        }

        return new UserGet()
        {
            Uuid = userEntity.Uuid,
            FirstName = userEntity.FirstName,
            LastName = userEntity.LastName,
            NickName = userEntity.NickName,
            Email = userEntity.Email,
            CreatedAt = userEntity.CreatedAt,
            UserRoleUuid = userRoleEntity.Uuid,
            UserRole = new UserRoleGet()
            {
                Uuid = userRoleEntity.Uuid,
                RoleName = userRoleEntity.RoleName
            }
        };
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