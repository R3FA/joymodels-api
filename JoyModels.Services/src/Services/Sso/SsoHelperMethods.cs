using System.Security.Cryptography;
using JoyModels.Models.DataTransferObjects.PendingUser;
using JoyModels.Models.DataTransferObjects.User;
using JoyModels.Services.Validation;
using Microsoft.AspNetCore.Identity;

namespace JoyModels.Services.Services.Sso;

public static class SsoHelperMethods
{
    private static readonly PasswordHasher<UserCreate> PasswordHasher = new();

    public static void ValidateUserCreation(this UserCreate user)
    {
        if (!RegularExpressionValidation.IsStringValid(user.FirstName,
                Validation.ConstantValidation.User.UserCreate.FirstNameMaxLength))
            throw new ArgumentException($"First name `{user.FirstName}` is invalid");

        if (user.LastName != null)
        {
            if (!RegularExpressionValidation.IsStringValid(user.LastName,
                    Validation.ConstantValidation.User.UserCreate.LastNameMaxLength))
                throw new ArgumentException($"Last name `{user.LastName}` is invalid");
        }

        if (!RegularExpressionValidation.IsNicknameValid(user.Nickname,
                Validation.ConstantValidation.User.UserCreate.NicknameMaxLength))
            throw new ArgumentException($"Nickname `{user.Nickname}` is invalid");

        if (!RegularExpressionValidation.IsEmailValid(user.Email,
                Validation.ConstantValidation.User.UserCreate.EmailMaxLength))
            throw new ArgumentException($"Email `{user.Email}` is invalid");

        if (!RegularExpressionValidation.IsPasswordValid(user.Password,
                Validation.ConstantValidation.User.UserCreate.PasswordMaxLength))
            throw new ArgumentException($"Password `{user.Password}` is invalid");
    }

    public static string GeneratePasswordHash(this UserCreate user, string password)
        => PasswordHasher.HashPassword(user, password);

    // TODO: You'll have to expand this logic when Login method comes
    public static PasswordVerificationResult VerifyPasswordHash(this UserCreate user, string hashedPassword,
        string password)
        => PasswordHasher.VerifyHashedPassword(user, hashedPassword, password);

    public static string GenerateOtpCode()
    {
        const string otpAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        const int otpCodeLength = 8;
        var chars = new char[otpCodeLength];

        var randomBytes = new byte[otpCodeLength];
        RandomNumberGenerator.Fill(randomBytes);

        for (var i = 0; i < otpCodeLength; i++)
        {
            chars[i] = otpAlphabet[randomBytes[i] % otpAlphabet.Length];
        }

        var otpCode = new string(chars);
        return !RegularExpressionValidation.IsOtpCodeValid(otpCode)
            ? throw new ArgumentException($"Generated OTP Code `{otpCode}` is invalid")
            : otpCode;
    }
}