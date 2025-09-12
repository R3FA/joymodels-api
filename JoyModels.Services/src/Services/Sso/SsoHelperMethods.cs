using System.Security.Cryptography;
using System.Transactions;
using JoyModels.Models.DataTransferObjects.Sso;
using JoyModels.Models.DataTransferObjects.User;
using JoyModels.Models.src.Database.Entities;
using JoyModels.Services.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.Sso;

public static class SsoHelperMethods
{
    private static readonly PasswordHasher<UserCreate> PasswordHasher = new();

    public static void ValidateUserCreation(this UserCreate user)
    {
        if (!RegularExpressionValidation.IsStringValid(user.FirstName,
                Validation.ConstantValidation.User.UserCreate.FirstNameMaxLength))
            throw new ArgumentException($"First name `{user.FirstName}` is invalid.");

        if (user.LastName != null)
        {
            if (!RegularExpressionValidation.IsStringValid(user.LastName,
                    Validation.ConstantValidation.User.UserCreate.LastNameMaxLength))
                throw new ArgumentException($"Last name `{user.LastName}` is invalid.");
        }

        if (!RegularExpressionValidation.IsNicknameValid(user.Nickname,
                Validation.ConstantValidation.User.UserCreate.NicknameMaxLength))
            throw new ArgumentException($"Nickname `{user.Nickname}` is invalid.");

        if (!RegularExpressionValidation.IsEmailValid(user.Email,
                Validation.ConstantValidation.User.UserCreate.EmailMaxLength))
            throw new ArgumentException($"Email `{user.Email}` is invalid.");

        if (!RegularExpressionValidation.IsPasswordValid(user.Password,
                Validation.ConstantValidation.User.UserCreate.PasswordMaxLength))
            throw new ArgumentException($"Password `{user.Password}` is invalid.");
    }

    public static void ValidateUuidValue(string uuid)
    {
        if (!Guid.TryParse(uuid, out var guid))
            throw new ArgumentException($"Sent value `{uuid}` has invalid UUID format.");
    }

    public static void ValidateOtpCodeValueFormat(string otpCode)
    {
        if (!RegularExpressionValidation.IsOtpCodeValid(otpCode))
            throw new ArgumentException($"Otp `{otpCode}` has invalid format.");
    }

    public static async Task ValidateOtpCodeForUserVerification(JoyModelsDbContext context,
        PendingUser pendingUserEntity,
        SsoVerify ssoVerifyDto)
    {
        if (pendingUserEntity.OtpCode != ssoVerifyDto.OtpCode)
            throw new ArgumentException("Invalid OTP code.");

        if (DateTime.Now > pendingUserEntity.OtpExpirationDate)
        {
            await GeneratePendingUserAfterOtpCodeExpiration(context, pendingUserEntity);
            throw new ArgumentException(
                "Sent OTP Code has expired. Another code has been generated and will be sent to your mail inbox");
        }
    }

    public static async Task<PendingUser> GetPendingUserEntity(JoyModelsDbContext context, SsoGet ssoGetDto)
    {
        var pendingUserEntity = await context.PendingUsers
            .Include(x => x.UserUu)
            .Include(x => x.UserUu!.UserRoleUu)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Uuid == Guid.Parse(ssoGetDto.PendingUserUuid)
                                      && x.UserUuid == Guid.Parse(ssoGetDto.UserUuid));

        return pendingUserEntity ?? throw new KeyNotFoundException("Pending user with sent values is not found.");
    }

    // TODO: Move this method when UserRole endpoint is created
    public static async Task<UserRole> GetUserRoleEntity(JoyModelsDbContext context, string roleName)
    {
        var userRoleEntity = await context.UserRoles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.RoleName == roleName);

        return userRoleEntity ??
               throw new KeyNotFoundException($"User role `{roleName}` is not found.");
    }

    // TODO: Move this method when Users endpoint is created
    public static async Task<User> GetUserEntity(JoyModelsDbContext context, string userUuid)
    {
        var userEntity = await context.Users
            .AsNoTracking()
            .Include(x => x.UserRoleUu)
            .FirstOrDefaultAsync(x => x.Uuid == Guid.Parse(userUuid));

        return userEntity ??
               throw new KeyNotFoundException($"User with UUID {userUuid} is not found.");
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
        pendingUserEntity.UserUu = null;

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
            ? throw new ArgumentException($"Generated OTP Code `{otpCode}` is invalid.")
            : otpCode;
    }

    private static async Task GeneratePendingUserAfterOtpCodeExpiration(JoyModelsDbContext context,
        PendingUser pendingUserEntity)
    {
        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await context.PendingUsers
                .Where(x => x.UserUuid == pendingUserEntity.UserUuid)
                .ExecuteDeleteAsync();
            await context.SaveChangesAsync();

            pendingUserEntity.SetCustomValuesPendingUserEntity();
            await context.PendingUsers.AddAsync(pendingUserEntity);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            throw new TransactionException(ex.InnerException!.Message);
        }
    }
}