using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.Users;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;
using JoyModels.Models.Enums;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using JoyModels.Services.Services.Sso.HelperMethods;
using JoyModels.Services.Validation;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.Users.HelperMethods;

public static class UsersHelperMethods
{
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

    public static async Task<PaginationBase<UserFollower>> SearchFollowingUsers(JoyModelsDbContext context,
        UserFollowerSearchRequest request)
    {
        var baseQuery = context.UserFollowers
            .AsNoTracking()
            .Include(x => x.UserTargetUu)
            .Include(x => x.UserTargetUu.UserRoleUu)
            .Where(x => x.UserTargetUu.UserRoleUu.RoleName != nameof(UserRoleEnum.Unverified)
                        && x.UserOriginUuid == request.TargetUserUuid);

        var filteredQuery = request.Nickname switch
        {
            not null => baseQuery.Where(x => x.UserTargetUu.NickName.Contains(request.Nickname)),
            _ => baseQuery
        };

        filteredQuery = GlobalHelperMethods<UserFollower>.OrderBy(filteredQuery, request.OrderBy);

        var userFollowing = await PaginationBase<UserFollower>.CreateAsync(filteredQuery,
            request.PageNumber,
            request.PageSize,
            request.OrderBy);

        return userFollowing;
    }

    public static async Task<PaginationBase<UserFollower>> SearchFollowerUsers(JoyModelsDbContext context,
        UserFollowerSearchRequest request)
    {
        var baseQuery = context.UserFollowers
            .AsNoTracking()
            .Include(x => x.UserOriginUu)
            .Include(x => x.UserOriginUu.UserRoleUu)
            .Where(x => x.UserOriginUu.UserRoleUu.RoleName != nameof(UserRoleEnum.Unverified)
                        && x.UserTargetUuid == request.TargetUserUuid);

        var filteredQuery = request.Nickname switch
        {
            not null => baseQuery.Where(x => x.UserOriginUu.NickName.Contains(request.Nickname)),
            _ => baseQuery
        };

        filteredQuery = GlobalHelperMethods<UserFollower>.OrderBy(filteredQuery, request.OrderBy);

        var userFollowerEntities = await PaginationBase<UserFollower>.CreateAsync(filteredQuery,
            request.PageNumber,
            request.PageSize,
            request.OrderBy);

        return userFollowerEntities;
    }

    public static async Task<PaginationBase<UserModelLike>> SearchUserModelLikes(JoyModelsDbContext context,
        UserModelLikesSearchRequest request)
    {
        var baseQuery = context.UserModelLikes
            .AsNoTracking()
            .Include(x => x.ModelUu)
            .Include(x => x.ModelUu.UserUu)
            .Include(x => x.ModelUu.UserUu.UserRoleUu)
            .Include(x => x.ModelUu.ModelAvailabilityUu)
            .Include(x => x.ModelUu.ModelCategories)
            .ThenInclude(x => x.CategoryUu)
            .Include(x => x.ModelUu.ModelPictures)
            .Where(x => x.UserUuid == request.UserUuid
                        && string.Equals(x.ModelUu.ModelAvailabilityUu.AvailabilityName,
                            nameof(ModelAvailabilityEnum.Public)));

        var filteredQuery = request.ModelName switch
        {
            not null => baseQuery.Where(x => x.ModelUu.Name.Contains(request.ModelName)),
            _ => baseQuery
        };

        filteredQuery = GlobalHelperMethods<UserModelLike>.OrderBy(filteredQuery, request.OrderBy);

        var userModelLikesEntity = await PaginationBase<UserModelLike>.CreateAsync(filteredQuery,
            request.PageNumber,
            request.PageSize,
            request.OrderBy);

        return userModelLikesEntity;
    }

    public static async Task CreateUserFollowerEntity(this UserFollower userFollowerEntity, JoyModelsDbContext context)
    {
        await context.AddAsync(userFollowerEntity);
        await context.SaveChangesAsync();
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

    public static async Task DeleteUserFollowerEntity(JoyModelsDbContext context, Guid targetUserUuid,
        UserAuthValidation userAuthValidation)
    {
        await context.UserFollowers
            .Where(x => x.UserOriginUuid == userAuthValidation.GetUserClaimUuid()
                        && x.UserTargetUuid == targetUserUuid)
            .ExecuteDeleteAsync();
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

        var totalRecords = await baseQuery.ExecuteDeleteAsync();

        if (totalRecords <= 0)
            throw new KeyNotFoundException(
                "User either doesn't exist or you don't own the account that you want to delete.");

        await context.SaveChangesAsync();
    }

    public static UserFollower CreateUserFollowerObject(Guid targetUserUuid, UserAuthValidation userAuthValidation)
    {
        return new UserFollower
        {
            Uuid = Guid.NewGuid(),
            UserOriginUuid = userAuthValidation.GetUserClaimUuid(),
            UserTargetUuid = targetUserUuid,
            FollowedAt = DateTime.Now
        };
    }
}