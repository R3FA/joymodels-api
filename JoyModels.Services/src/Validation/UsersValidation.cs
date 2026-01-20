using System.Data;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.Users;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Validation;

public static class UsersValidation
{
    public static void ValidateUserSearchArguments(this UsersSearchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Nickname))
            RegularExpressionValidation.ValidateNickname(request.Nickname);
    }

    public static void ValidateUserSearchFollowingUsersArguments(this UserFollowerSearchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Nickname))
            RegularExpressionValidation.ValidateNickname(request.Nickname);
    }

    public static void ValidateUserModelLikesSearchArguments(this UserModelLikesSearchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.ModelName))
            RegularExpressionValidation.ValidateText(request.ModelName);
    }

    public static async Task ValidateUserFollowEndpoint(JoyModelsDbContext context, Guid targetUserUuid,
        UserAuthValidation userAuthValidation)
    {
        if (userAuthValidation.GetUserClaimUuid() == targetUserUuid)
            throw new ArgumentException("You cannot follow yourself.");

        var exists = await context.UserFollowers
            .AnyAsync(x =>
                x.UserOriginUuid == userAuthValidation.GetUserClaimUuid() && x.UserTargetUuid == targetUserUuid);
        if (exists)
            throw new ArgumentException("You have already followed this user.");
    }

    public static void ValidateUserPatchArguments(this UsersPatchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName)
            && string.IsNullOrWhiteSpace(request.LastName)
            && string.IsNullOrWhiteSpace(request.Nickname)
            && request.UserPicture == null)
            throw new ArgumentException("You cannot send an empty request!");

        if (!string.IsNullOrWhiteSpace(request.FirstName))
            RegularExpressionValidation.ValidateName(request.FirstName);

        if (!string.IsNullOrWhiteSpace(request.LastName))
            RegularExpressionValidation.ValidateName(request.LastName);

        if (!string.IsNullOrWhiteSpace(request.Nickname))
            RegularExpressionValidation.ValidateNickname(request.Nickname);
    }

    public static async Task ValidateUserPatchArgumentsDuplicatedFields(this UsersPatchRequest request,
        JoyModelsDbContext context)
    {
        if (!string.IsNullOrWhiteSpace(request.Nickname))
        {
            var isNicknameDuplicated = await context.Users.AnyAsync(x => x.NickName == request.Nickname);
            if (isNicknameDuplicated)
                throw new DuplicateNameException(
                    $"Nickname `{request.Nickname}` is already registered in our database.");
        }
    }

    public static async Task ValidateUserUnfollowEndpoint(JoyModelsDbContext context, Guid targetUserUuid,
        UserAuthValidation userAuthValidation)
    {
        if (userAuthValidation.GetUserClaimUuid() == targetUserUuid)
            throw new ArgumentException("You cannot unfollow yourself.");

        var exists = await context.UserFollowers
            .AnyAsync(x =>
                x.UserOriginUuid == userAuthValidation.GetUserClaimUuid() && x.UserTargetUuid == targetUserUuid);
        if (!exists)
            throw new ArgumentException("You don't follow this user.");
    }
}