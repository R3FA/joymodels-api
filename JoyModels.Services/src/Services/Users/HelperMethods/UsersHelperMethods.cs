using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using UserRoleEnum = JoyModels.Models.Enums.UserRole;

namespace JoyModels.Services.Services.Users.HelperMethods;

public static class UsersHelperMethods
{
    public static async Task<User> GetUserEntity(JoyModelsDbContext context, Guid userUuid)
    {
        var pendingUserEntity = await context.Users
            .AsNoTracking()
            .Include(x => x.UserRoleUu)
            .Where(x => x.UserRoleUu.RoleName != nameof(UserRoleEnum.Undefined)
                        && x.UserRoleUu.RoleName != nameof(UserRoleEnum.Unverified))
            .FirstOrDefaultAsync(x => x.Uuid == userUuid);

        return pendingUserEntity ?? throw new KeyNotFoundException("User with sent values is not found.");
    }
}