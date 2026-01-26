using System.Data;
using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.Sso;
using JoyModels.Services.Services.Sso.HelperMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace JoyModels.Services.Validation;

public static class SsoValidation
{
    public static void ValidateUserCreationArguments(this SsoUserCreateRequest request)
    {
        RegularExpressionValidation.ValidateName(request.FirstName);

        if (!string.IsNullOrWhiteSpace(request.LastName))
            RegularExpressionValidation.ValidateName(request.LastName);

        RegularExpressionValidation.ValidateNickname(request.Nickname);
        RegularExpressionValidation.ValidateEmail(request.Email);
        RegularExpressionValidation.ValidatePassword(request.Password);
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

    public static async Task ValidateUserPicture(IFormFile userPicture,
        UserImageSettingsDetails userImageSettingsDetails)
    {
        if (userPicture.Length > userImageSettingsDetails.AllowedSize)
            throw new ArgumentException("Image too large. Maximum size limit is 10MB");

        await using (var s1 = userPicture.OpenReadStream())
        {
            var format = await Image.DetectFormatAsync(s1);
            if (format is null || (format != JpegFormat.Instance && format != PngFormat.Instance))
                throw new ArgumentException("Unsupported image format. Allowed: .jpg, .jpeg, .png");
        }

        await using var s2 = userPicture.OpenReadStream();
        var info = await Image.IdentifyAsync(s2);
        if (info == null)
            throw new ArgumentException("Unsupported or corrupted image.");

        var minWidth = userImageSettingsDetails.ImageSettingsResolutionDetails.MinimumWidth;
        var maxWidth = userImageSettingsDetails.ImageSettingsResolutionDetails.MaximumWidth;
        var minHeight = userImageSettingsDetails.ImageSettingsResolutionDetails.MinimumHeight;
        var maxHeight = userImageSettingsDetails.ImageSettingsResolutionDetails.MaximumHeight;

        if (info.Width < minWidth || info.Width > maxWidth || info.Height < minHeight || info.Height > maxHeight)
            throw new ArgumentException(
                $"Image error: {info.Width}x{info.Height}. Allowed: width between {minWidth}-{maxWidth}px and height between {minHeight}-{maxHeight}px.");
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
            RegularExpressionValidation.ValidateText(request.Nickname);

        if (!string.IsNullOrWhiteSpace(request.Email))
            RegularExpressionValidation.ValidateText(request.Email);
    }

    public static void ValidateUserPasswordChangeRequestArguments(
        this SsoPasswordChangeRequest passwordChangeRequest)
    {
        if (passwordChangeRequest.NewPassword != passwordChangeRequest.ConfirmNewPassword)
            throw new ArgumentException("New password and confirm password do not match.");

        RegularExpressionValidation.ValidatePassword(passwordChangeRequest.NewPassword);
    }

    public static void ValidateUserLoginRequestArguments(this SsoLoginRequest request)
    {
        RegularExpressionValidation.ValidateNickname(request.Nickname);
        RegularExpressionValidation.ValidatePassword(request.Password);
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
}