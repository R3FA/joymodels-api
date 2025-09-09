using System.Security.Cryptography;
using JoyModels.Models.DataTransferObjects.User;
using JoyModels.Models.src.Database.Entities;
using JoyModels.Services.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserRoleEnum = JoyModels.Models.Enums.UserRole;

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

    public static void ValidateUuidValue(string uuid)
    {
        if (!Guid.TryParse(uuid, out var guid))
            throw new ArgumentException($"Sent value `{uuid}` has invalid UUID format");
    }

    public static async Task<PendingUser> GetPendingUserEntity(JoyModelsDbContext context, string uuid)
    {
        var pendingUserEntity = await context.PendingUsers
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Uuid.ToString() == uuid || x.UserUuid.ToString() == uuid);

        return pendingUserEntity ?? throw new KeyNotFoundException($"Pending user with uuid `{uuid}` is not found");
    }

    public static async Task<UserRole> GetUserRoleEntity(JoyModelsDbContext context)
    {
        var userRoleEntity = await context.UserRoles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.RoleName == nameof(UserRoleEnum.Unverified));

        return userRoleEntity ??
               throw new KeyNotFoundException($"User Role {nameof(UserRoleEnum.Unverified)} is not found");
    }

    public static User SetCustomValuesUserEntity(this User userEntity, UserCreate userDto, UserRole userRole)
    {
        userEntity.Uuid = Guid.NewGuid();
        userEntity.PasswordHash = userDto.GeneratePasswordHash(userDto.Password);
        userEntity.CreatedAt = DateTime.Now;
        userEntity.UserRoleUuid = userRole.Uuid;

        return userEntity;
    }

    public static PendingUser SetCustomValuesPendingUserEntity(this PendingUser pendingUserEntity)
    {
        pendingUserEntity.Uuid = Guid.NewGuid();
        pendingUserEntity.OtpCode = GenerateOtpCode();
        pendingUserEntity.OtpCreatedAt = DateTime.Now;
        pendingUserEntity.OtpExpirationDate = DateTime.Now.AddMinutes(60);

        return pendingUserEntity;
    }

    private static string GeneratePasswordHash(this UserCreate user, string password)
        => PasswordHasher.HashPassword(user, password);

    // TODO: You'll have to expand this logic when Login method comes
    private static PasswordVerificationResult VerifyPasswordHash(this UserCreate user, string hashedPassword,
        string password)
        => PasswordHasher.VerifyHashedPassword(user, hashedPassword, password);

    private static string GenerateOtpCode()
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