using System.Data;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.Users;
using Microsoft.EntityFrameworkCore;

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

    public static void ValidateUserSearchArguments(this UsersSearchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Nickname))
            ValidateNickname(request.Nickname);
    }

    public static async Task ValidateUserFollowEndpoint(JoyModelsDbContext context, UsersFollowRequest request)
    {
        if (request.OriginUserUuid == request.TargetUserUuid)
            throw new ArgumentException("You cannot follow yourself.");

        var exists = await context.UserFollowers
            .AnyAsync(x => x.UserOriginUuid == request.OriginUserUuid && x.UserTargetUuid == request.TargetUserUuid);
        if (exists)
            throw new ArgumentException("You already follow this user.");
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
}