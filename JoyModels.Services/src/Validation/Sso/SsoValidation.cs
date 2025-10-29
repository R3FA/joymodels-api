using System.Data;
using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.Sso;
using JoyModels.Services.Services.Sso.HelperMethods;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Validation.Sso;

public static class SsoValidation
{
    public static void ValidateUserCreationArguments(this SsoUserCreateRequest request)
    {
        if (!RegularExpressionValidation.IsNameValid(request.FirstName))
            throw new ArgumentException(
                "First name must begin with a capital letter and contain only lowercase letters after.");

        if (!string.IsNullOrWhiteSpace(request.LastName))
            if (!RegularExpressionValidation.IsNameValid(request.LastName))
                throw new ArgumentException(
                    "Last name must begin with a capital letter and contain only lowercase letters after.");

        ValidateNickname(request.Nickname);
        ValidateEmail(request.Email);
        ValidatePassword(request.Password);
    }

    public static async Task ValidateUserCreationDuplicatedFields(this SsoUserCreateRequest request,
        JoyModelsDbContext context)
    {
        var isNicknameDuplicated = await context.Users.AnyAsync(x => x.NickName == request.Nickname);
        var isEmailDuplicated = await context.Users.AnyAsync(x => x.Email == request.Email);

        if (isNicknameDuplicated)
            throw new DuplicateNameException($"Nickname `{request.Nickname}` is already registered in our database.");

        if (isEmailDuplicated)
            throw new DuplicateNameException($"Email `{request.Email}` is already registered in our database.");
    }

    public static void ValidateOtpCodeValueFormat(string otpCode)
    {
        if (!RegularExpressionValidation.IsOtpCodeValid(otpCode))
            throw new ArgumentException("OTP code must be 12 characters, using only uppercase letters and numbers.");
    }

    public static void ValidateOtpCodeForUserVerification(PendingUser pendingUserEntity,
        SsoVerifyRequest request)
    {
        if (pendingUserEntity.OtpCode != request.OtpCode)
            throw new ArgumentException("Invalid OTP code.");

        if (DateTime.Now > pendingUserEntity.OtpExpirationDate)
            throw new ArgumentException(
                "Sent OTP Code has expired. Click on resend verification button to regenerate a new OTP code.");
    }

    public static void ValidateUserSearchArguments(this SsoSearchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Nickname))
            ValidateNickname(request.Nickname);

        if (!string.IsNullOrWhiteSpace(request.Email))
            ValidateEmail(request.Email);
    }

    public static void ValidateUserPasswordChangeRequestArguments(
        this SsoPasswordChangeRequest passwordChangeRequest)
    {
        if (passwordChangeRequest.NewPassword != passwordChangeRequest.ConfirmNewPassword)
            throw new ArgumentException("New password and confirm password do not match.");

        ValidatePassword(passwordChangeRequest.NewPassword);
    }

    public static void ValidateUserLoginRequestArguments(this SsoLoginRequest request)
    {
        ValidateNickname(request.Nickname);
        ValidatePassword(request.Password);
    }

    public static void ValidateUsersPassword(this SsoLoginRequest request, User userEntity)
    {
        var passwordVerificationResult =
            SsoPasswordHasher.Verify(userEntity, userEntity.PasswordHash, request.Password);

        if (passwordVerificationResult is PasswordVerificationResult.Failed)
            throw new ArgumentException("User password is incorrect");
    }

    public static async Task ValidateUserRefreshToken(this SsoAccessTokenChangeRequest accessTokenChangeRequest,
        JoyModelsDbContext context, IMapper mapper)
    {
        var userTokenEntity = await context.UserTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.UserUuid == accessTokenChangeRequest.UserUuid &&
                x.RefreshToken == accessTokenChangeRequest.UserRefreshToken);

        if (userTokenEntity == null)
            throw new KeyNotFoundException("Users refresh token does not exist.");

        if (DateTime.Now >= userTokenEntity.TokenExpirationDate)
        {
            var ssoLogoutRequest = mapper.Map<SsoLogoutRequest>(accessTokenChangeRequest);
            await ssoLogoutRequest.DeleteUserRefreshToken(context);
            throw new ArgumentException("Refresh token is expired. Logging user out of the application.");
        }
    }

    public static void ValidateIfUserHasSameRole(this User userEntity, UserRole designatedRole)
    {
        if (userEntity.UserRoleUuid == designatedRole.Uuid)
            throw new ArgumentException($"User {userEntity.NickName} already has a role {designatedRole.RoleName}");
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

    private static void ValidatePassword(string password)
    {
        if (!RegularExpressionValidation.IsPasswordValid(password))
            throw new ArgumentException(
                "Password must have at least 8 characters, one uppercase letter, one number, and one special character (!@#$%^&*).");
    }
}