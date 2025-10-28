using System.Data;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.Users;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Validation.Users;

public static class UsersValidation
{
    private static void ValidateFirstName(string firstName)
    {
        if (!RegularExpressionValidation.IsStringValid(firstName))
            throw new ArgumentException(
                "First name must begin with a capital letter and contain only lowercase letters after.");
    }

    private static void ValidateLastName(string lastName)
    {
        if (!RegularExpressionValidation.IsStringValid(lastName))
            throw new ArgumentException(
                "Last name must begin with a capital letter and contain only lowercase letters after.");
    }

    private static void ValidateNickname(string nickname)
    {
        if (!RegularExpressionValidation.IsNicknameValid(nickname))
            throw new ArgumentException(
                "Nickname must have at least 3 characters and may only contain lowercase letters and numbers.");
    }

    private static void ValidateEmail(string email)
    {
        if (!RegularExpressionValidation.IsEmailValid(email))
            throw new ArgumentException(
                "Email must contain the '@' symbol, followed by a domain with a dot. Value has to be without spaces or blank characters.");
    }

    public static void ValidateUserSearchArguments(this UsersSearchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Nickname))
            ValidateNickname(request.Nickname);
    }

    public static void ValidateUserPatchArguments(this UsersPatchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName)
            && string.IsNullOrWhiteSpace(request.LastName)
            && string.IsNullOrWhiteSpace(request.Nickname)
            && string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("You cannot send an empty request!");

        if (!string.IsNullOrWhiteSpace(request.FirstName))
            ValidateFirstName(request.FirstName);

        if (!string.IsNullOrWhiteSpace(request.LastName))
            ValidateLastName(request.LastName);

        if (!string.IsNullOrWhiteSpace(request.Nickname))
            ValidateNickname(request.Nickname);

        if (!string.IsNullOrWhiteSpace(request.Email))
            ValidateEmail(request.Email);
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

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var isEmailDuplicated = await context.Users.AnyAsync(x => x.Email == request.Email);
            if (isEmailDuplicated)
                throw new DuplicateNameException($"Email `{request.Email}` is already registered in our database.");
        }
    }
}