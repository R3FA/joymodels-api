using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.Users;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using Microsoft.EntityFrameworkCore;
using UserRoleEnum = JoyModels.Models.Enums.UserRole;

namespace JoyModels.Services.Services.Users.HelperMethods;

public static class UsersHelperMethods
{
    public static async Task<User> GetUserEntity(JoyModelsDbContext context, Guid userUuid)
    {
        var userEntity = await context.Users
            .AsNoTracking()
            .Include(x => x.UserRoleUu)
            .Where(x => x.UserRoleUu.RoleName != nameof(UserRoleEnum.Undefined)
                        && x.UserRoleUu.RoleName != nameof(UserRoleEnum.Unverified))
            .FirstOrDefaultAsync(x => x.Uuid == userUuid);

        return userEntity ?? throw new KeyNotFoundException("User with sent values is not found.");
    }

    public static async Task<PaginationBase<User>> SearchUserEntities(JoyModelsDbContext context,
        UsersSearchRequest usersSearchRequestDto)
    {
        var baseQuery = context.Users
            .AsNoTracking()
            .Include(x => x.UserRoleUu)
            .Where(x => x.UserRoleUu.RoleName != nameof(UserRoleEnum.Undefined)
                        && x.UserRoleUu.RoleName != nameof(UserRoleEnum.Unverified));

        var filteredQuery = usersSearchRequestDto.Nickname switch
        {
            not null => baseQuery.Where(x => x.NickName.Contains(usersSearchRequestDto.Nickname)),
            _ => baseQuery
        };

        filteredQuery = GlobalHelperMethods<User>.OrderBy(filteredQuery, usersSearchRequestDto.OrderBy);

        var userEntities = await PaginationBase<User>.CreateAsync(filteredQuery,
            usersSearchRequestDto.PageNumber,
            usersSearchRequestDto.PageSize,
            usersSearchRequestDto.OrderBy);

        return userEntities;
    }

    public static async Task PatchUserEntity(this UsersPatchRequest request, JoyModelsDbContext context)
    {
        if (!string.IsNullOrWhiteSpace(request.FirstName))
            await context.Users.ExecuteUpdateAsync(x => x.SetProperty(z => z.FirstName, request.FirstName));

        if (!string.IsNullOrWhiteSpace(request.LastName))
            await context.Users.ExecuteUpdateAsync(x => x.SetProperty(z => z.LastName, request.LastName));

        if (!string.IsNullOrWhiteSpace(request.Nickname))
            await context.Users.ExecuteUpdateAsync(x => x.SetProperty(z => z.NickName, request.Nickname));

        if (!string.IsNullOrWhiteSpace(request.Email))
            await context.Users.ExecuteUpdateAsync(x => x.SetProperty(z => z.Email, request.Email));

        await context.SaveChangesAsync();
    }

    public static async Task DeleteUserEntity(JoyModelsDbContext context, Guid userUuid)
    {
        var numberOfDeletedRows = await context.Users
            .Where(x => x.Uuid == userUuid)
            .ExecuteDeleteAsync();
        await context.SaveChangesAsync();

        if (numberOfDeletedRows == 0)
            throw new KeyNotFoundException(
                $"Unverified user with UUID `{userUuid}` either is verified or does not exist.");
    }
}