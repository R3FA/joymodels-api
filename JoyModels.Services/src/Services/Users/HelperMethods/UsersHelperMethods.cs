using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.Users;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using JoyModels.Services.Services.Sso.HelperMethods;
using JoyModels.Services.Validation;
using Microsoft.EntityFrameworkCore;
using UserRoleEnum = JoyModels.Models.Enums.UserRole;

namespace JoyModels.Services.Services.Users.HelperMethods;

public static class UsersHelperMethods
{
    public static async Task<int> GetUserFollowing(JoyModelsDbContext context, Guid userUuid)
    {
        return await context.UserFollowers
            .AsNoTracking()
            .Where(x => x.UserOriginUuid == userUuid)
            .Distinct()
            .CountAsync();
    }

    public static async Task<int> GetUserFollowers(JoyModelsDbContext context, Guid userUuid)
    {
        return await context.UserFollowers
            .AsNoTracking()
            .Where(x => x.UserTargetUuid == userUuid)
            .Distinct()
            .CountAsync();
    }

    public static async Task<User> GetUserEntity(JoyModelsDbContext context, Guid userUuid)
    {
        var userEntity = await context.Users
            .AsNoTracking()
            .Include(x => x.UserRoleUu)
            .Where(x => x.UserRoleUu.RoleName != nameof(UserRoleEnum.Unverified))
            .FirstOrDefaultAsync(x => x.Uuid == userUuid);

        return userEntity ?? throw new KeyNotFoundException("User with sent values is not found.");
    }

    public static async Task<PaginationBase<User>> SearchUserEntities(JoyModelsDbContext context,
        UsersSearchRequest usersSearchRequestDto)
    {
        var baseQuery = context.Users
            .AsNoTracking()
            .Include(x => x.UserRoleUu)
            .Where(x => x.UserRoleUu.RoleName != nameof(UserRoleEnum.Unverified));

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

    public static async Task PatchUserEntity(this UsersPatchRequest request, JoyModelsDbContext context,
        UsersResponse userResponse, UserImageSettingsDetails userImageSettingsDetails)
    {
        if (!string.IsNullOrWhiteSpace(request.FirstName))
            await context.Users.Where(x => x.Uuid == request.UserUuid)
                .ExecuteUpdateAsync(x => x.SetProperty(z => z.FirstName, request.FirstName));

        if (!string.IsNullOrWhiteSpace(request.LastName))
            await context.Users.Where(x => x.Uuid == request.UserUuid)
                .ExecuteUpdateAsync(x => x.SetProperty(z => z.LastName, request.LastName));

        if (!string.IsNullOrWhiteSpace(request.Nickname))
            await context.Users.Where(x => x.Uuid == request.UserUuid)
                .ExecuteUpdateAsync(x => x.SetProperty(z => z.NickName, request.Nickname));

        if (request.UserPicture != null)
        {
            var userPicturePath = string.Empty;
            try
            {
                userPicturePath =
                    await SsoHelperMethods.SaveUserPicture(request.UserPicture, userImageSettingsDetails,
                        request.UserUuid);

                SsoHelperMethods.DeleteUserPictureOnException(userResponse.UserPictureLocation);

                await context.Users.Where(x => x.Uuid == request.UserUuid)
                    .ExecuteUpdateAsync(x => x.SetProperty(z => z.UserPictureLocation, userPicturePath));
            }
            catch (Exception e)
            {
                SsoHelperMethods.DeleteUserPictureOnException(userPicturePath);
                throw new Exception(e.Message);
            }
        }

        await context.SaveChangesAsync();
    }

    public static async Task DeleteUserEntity(JoyModelsDbContext context, Guid userUuid,
        UserAuthValidation userAuthValidation)
    {
        var baseQuery = context.Users.AsQueryable();

        baseQuery = userAuthValidation.GetUserClaimRole() switch
        {
            nameof(UserRoleEnum.Admin) or nameof(UserRoleEnum.Root) => baseQuery.Where(x => x.Uuid == userUuid),
            _ => baseQuery.Where(x => x.Uuid == userUuid && x.Uuid == userAuthValidation.GetUserClaimUuid())
        };

        var totalCount = await baseQuery.ExecuteDeleteAsync();

        if (totalCount <= 0)
            throw new KeyNotFoundException(
                "User either doesn't exist or you don't own the account that you want to delete.");
    }
}