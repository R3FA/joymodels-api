using System.Data;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.Users;
using Microsoft.EntityFrameworkCore;
using ModelAvailabilityEnum = JoyModels.Models.Enums.ModelAvailability;

namespace JoyModels.Services.Validation.Users;

public static class UsersValidation
{
    private static void ValidateFirstName(string firstName)
    {
        if (!RegularExpressionValidation.IsNameValid(firstName))
            throw new ArgumentException(
                "First name must begin with a capital letter and contain only lowercase letters after.");
    }

    private static void ValidateLastName(string lastName)
    {
        if (!RegularExpressionValidation.IsNameValid(lastName))
            throw new ArgumentException(
                "Last name must begin with a capital letter and contain only lowercase letters after.");
    }

    private static void ValidateNickname(string nickname)
    {
        if (!RegularExpressionValidation.IsNicknameValid(nickname))
            throw new ArgumentException(
                "Nickname must have at least 3 characters and may only contain lowercase letters and numbers.");
    }

    private static void ValidateModelName(string modelName)
    {
        if (!RegularExpressionValidation.IsStringValid(modelName))
            throw new ArgumentException(
                "Invalid value: Must contain only letters (any language), digits, and the following characters: ':', '.', ',', '-'.");
    }

    public static void ValidateUserSearchArguments(this UsersSearchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Nickname))
            ValidateNickname(request.Nickname);
    }

    public static void ValidateUserSearchFollowingUsersArguments(this UserFollowerSearchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Nickname))
            ValidateNickname(request.Nickname);
    }

    public static void ValidateUserModelLikesSearchArguments(this UserModelLikesSearchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.ModelName))
            ValidateModelName(request.ModelName);
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

    public static async Task ValidateModelLikeEndpoint(JoyModelsDbContext context, Guid modelUuid,
        UserAuthValidation userAuthValidation)
    {
        var isModelPublic = await context.UserModelLikes
            .Include(x => x.ModelUu)
            .Include(x => x.ModelUu.ModelAvailabilityUu)
            .AnyAsync(x => x.ModelUuid == modelUuid
                           && x.ModelUu.ModelAvailabilityUu.AvailabilityName == nameof(ModelAvailabilityEnum.Public));

        if (!isModelPublic)
            throw new ArgumentException("You cannot like a hidden model.");

        var exists = await context.UserModelLikes
            .AnyAsync(x =>
                x.UserUuid == userAuthValidation.GetUserClaimUuid()
                && x.ModelUuid == modelUuid);
        if (exists)
            throw new ArgumentException("You have already liked this model.");
    }

    public static void ValidateUserPatchArguments(this UsersPatchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName)
            && string.IsNullOrWhiteSpace(request.LastName)
            && string.IsNullOrWhiteSpace(request.Nickname))
            throw new ArgumentException("You cannot send an empty request!");

        if (!string.IsNullOrWhiteSpace(request.FirstName))
            ValidateFirstName(request.FirstName);

        if (!string.IsNullOrWhiteSpace(request.LastName))
            ValidateLastName(request.LastName);

        if (!string.IsNullOrWhiteSpace(request.Nickname))
            ValidateNickname(request.Nickname);
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

    public static async Task ValidateModelUnlikeEndpoint(JoyModelsDbContext context, Guid modelUuid,
        UserAuthValidation userAuthValidation)
    {
        var exists = await context.UserModelLikes
            .AnyAsync(x =>
                x.UserUuid == userAuthValidation.GetUserClaimUuid() && x.ModelUuid == modelUuid);
        if (!exists)
            throw new ArgumentException("You cannot unlike a model that you have never liked.");
    }
}