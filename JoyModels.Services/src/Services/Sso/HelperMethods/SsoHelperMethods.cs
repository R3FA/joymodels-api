using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.Jwt;
using JoyModels.Models.DataTransferObjects.RequestTypes.Email;
using JoyModels.Models.DataTransferObjects.RequestTypes.Sso;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Sso;
using JoyModels.Models.Enums;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using JoyModels.Services.Validation;
using JoyModels.Utilities.RabbitMQ.MessageProducer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JoyModels.Services.Services.Sso.HelperMethods;

public static class SsoHelperMethods
{
    private static string CreateUserJwtAccessToken(User user, JwtClaimDetails jwtClaimDetails)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Uuid.ToString()),
            new(ClaimTypes.Name, user.NickName),
            new(ClaimTypes.Role, user.UserRoleUu.RoleName)
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtClaimDetails.JwtSigningKey));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512);
        var signingKeyDescriptor = new JwtSecurityToken(
            issuer: jwtClaimDetails.JwtIssuer,
            audience: jwtClaimDetails.JwtAudience,
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(signingKeyDescriptor);
    }

    private static string CreateUserJwtRefreshToken()
    {
        var randomBytes = new byte[64];
        RandomNumberGenerator.Create().GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private static string GenerateOtpCode()
    {
        const string otpAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        const int otpCodeLength = 12;
        var chars = new char[otpCodeLength];

        var randomBytes = new byte[otpCodeLength];
        RandomNumberGenerator.Fill(randomBytes);

        for (var i = 0; i < otpCodeLength; i++)
            chars[i] = otpAlphabet[randomBytes[i] % otpAlphabet.Length];

        var otpCode = new string(chars);
        RegularExpressionValidation.ValidateOtpCode(otpCode);
        return otpCode;
    }

    public static async Task<PendingUser> GetPendingUserEntity(JoyModelsDbContext context, Guid userUuid)
    {
        var pendingUserEntity = await context.PendingUsers
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .FirstOrDefaultAsync(x => x.UserUuid == userUuid);

        return pendingUserEntity ?? throw new KeyNotFoundException("Pending user with sent values is not found.");
    }

    public static async Task<PaginationBase<User>> SearchPendingUserEntities(JoyModelsDbContext context,
        SsoSearchRequest ssoSearchRequestDto)
    {
        var baseQuery = context.PendingUsers
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .Select(x => x.UserUu);

        var filteredQuery = (ssoSearchRequestDto.Nickname, ssoSearchRequestDto.Email) switch
        {
            (not null, null) => baseQuery.Where(x => x.NickName.Contains(ssoSearchRequestDto.Nickname)),
            (null, not null) => baseQuery.Where(x => x.Email.Contains(ssoSearchRequestDto.Email)),
            (not null, not null) => baseQuery.Where(x =>
                x.NickName.Contains(ssoSearchRequestDto.Nickname) &&
                x.Email.Contains(ssoSearchRequestDto.Email)),
            _ => baseQuery
        };

        filteredQuery = GlobalHelperMethods<User>.OrderBy(filteredQuery, ssoSearchRequestDto.OrderBy);

        var unverifiedUserEntities = await PaginationBase<User>.CreateAsync(filteredQuery,
            ssoSearchRequestDto.PageNumber,
            ssoSearchRequestDto.PageSize,
            ssoSearchRequestDto.OrderBy);

        return unverifiedUserEntities;
    }

    public static async Task<JoyModels.Models.Database.Entities.UserRole> GetUserRoleEntity(JoyModelsDbContext context,
        Guid? roleUuid, string? roleName)
    {
        var baseQuery = context.UserRoles.AsNoTracking();

        var userRoleEntity = (roleUuid, roleName) switch
        {
            (not null, null) => await baseQuery.FirstOrDefaultAsync(x => x.Uuid == roleUuid),
            (null, not null) => await baseQuery.FirstOrDefaultAsync(x => x.RoleName == roleName),
            (not null, not null) => await baseQuery.FirstOrDefaultAsync(x =>
                x.Uuid == roleUuid &&
                x.RoleName == roleName),
            _ => throw new ArgumentException("Invalid arguments")
        };

        return userRoleEntity ??
               throw new KeyNotFoundException("User role is not found.");
    }

    public static async Task<User> GetUserEntity(JoyModelsDbContext context, Guid? userUuid,
        string? userNickname)
    {
        var baseQuery = context.Users
            .AsNoTracking()
            .Include(x => x.UserRoleUu);

        var userEntity = (userUuid, userNickname) switch
        {
            (not null, null) => await baseQuery.FirstOrDefaultAsync(x => x.Uuid == userUuid),
            (null, not null) => await baseQuery.FirstOrDefaultAsync(x => x.NickName == userNickname),
            (not null, not null) => await baseQuery.FirstOrDefaultAsync(x =>
                x.Uuid == userUuid &&
                x.NickName == userNickname),
            _ => throw new ArgumentException("Invalid arguments")
        };

        return userEntity ??
               throw new KeyNotFoundException("User with sent values is not found.");
    }

    public static async Task<User> GetUserAdminEntity(JoyModelsDbContext context, SsoLoginRequest request)
    {
        var userEntity = await context.Users
            .AsNoTracking()
            .Include(x => x.UserRoleUu)
            .Where(x => x.UserRoleUu.RoleName == nameof(UserRoleEnum.Admin)
                        || x.UserRoleUu.RoleName == nameof(UserRoleEnum.Root))
            .FirstOrDefaultAsync(x => string.Equals(x.NickName, request.Nickname));

        return userEntity ??
               throw new KeyNotFoundException("Admin with sent values is not found.");
    }

    public static async Task CheckIfUserExists(JoyModelsDbContext context, Guid userUuid)
    {
        var userExists = await context.Users
            .AsNoTracking()
            .AnyAsync(x => x.Uuid == userUuid);

        if (!userExists)
            throw new KeyNotFoundException(
                $"User with UUID `{userUuid}` either is unverified or does not exist.");
    }

    public static void SetUserRoleValidation(this User userEntity)
    {
        if (userEntity.UserRoleUu.RoleName is nameof(UserRoleEnum.Unverified))
            throw new ApplicationException("User is unverified.");

        if (userEntity.UserRoleUu.RoleName is nameof(UserRoleEnum.Root))
            throw new ApplicationException("You cannot set another role for root admin.");
    }

    public static User SetCustomValuesUserEntity(this User userEntity, SsoUserCreateRequest request,
        JoyModels.Models.Database.Entities.UserRole userRoleEntity)
    {
        userEntity.Uuid = Guid.NewGuid();
        userEntity.PasswordHash = SsoPasswordHasher.Hash(request, request.Password);
        userEntity.CreatedAt = DateTime.Now;
        userEntity.UserRoleUuid = userRoleEntity.Uuid;

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

    public static SsoLoginResponse SetCustomValuesSsoLoginResponse(User userEntity, JwtClaimDetails jwtClaimDetails)
    {
        return new SsoLoginResponse
        {
            AccessToken = CreateUserJwtAccessToken(userEntity, jwtClaimDetails),
            RefreshToken = CreateUserJwtRefreshToken()
        };
    }

    public static UserToken SetCustomValuesUserTokenEntity(User userEntity, SsoLoginResponse ssoLoginResponse)
    {
        return new UserToken
        {
            Uuid = Guid.NewGuid(),
            UserUuid = userEntity.Uuid,
            RefreshToken = ssoLoginResponse.RefreshToken,
            TokenExpirationDate = DateTime.Now.AddDays(90)
        };
    }

    public static SsoAccessTokenChangeResponse SetCustomValuesSsoAccessTokenChangeResponse(
        User userEntity, JwtClaimDetails jwtClaimDetails)
    {
        return new SsoAccessTokenChangeResponse
        {
            UserAccessToken = CreateUserJwtAccessToken(userEntity, jwtClaimDetails),
        };
    }

    public static async Task CreateUser(this User userEntity, JoyModelsDbContext context)
    {
        await context.Users.AddAsync(userEntity);
        await context.SaveChangesAsync();
    }

    public static async Task<string> SaveUserPicture(IFormFile userPicture,
        UserImageSettingsDetails userImageSettingsDetails, Guid userUuid)
    {
        try
        {
            await SsoValidation.ValidateUserPicture(userPicture, userImageSettingsDetails);

            var userPictureName = $"user-picture-{Guid.NewGuid()}{Path.GetExtension(userPicture.FileName)}";

            var basePath =
                Directory.CreateDirectory(Path.Combine(userImageSettingsDetails.SavePath, "users",
                    userUuid.ToString()));
            var userPicturePath = Path.Combine(basePath.FullName, userPictureName);

            await using var stream = new FileStream(userPicturePath, FileMode.Create);
            await userPicture.CopyToAsync(stream);

            return userPictureName;
        }
        catch (Exception e)
        {
            throw new ApplicationException($"Failed to save user picture: {e.Message}");
        }
    }

    public static async Task CreatePendingUser(this PendingUser pendingUserEntity, JoyModelsDbContext context)
    {
        await context.PendingUsers.AddAsync(pendingUserEntity);
        await context.SaveChangesAsync();
    }

    public static async Task CreateUserToken(this UserToken tokenEntity, JoyModelsDbContext context)
    {
        await context.UserTokens.AddAsync(tokenEntity);
        await context.SaveChangesAsync();
    }

    public static async Task UpdateUsersRole(JoyModelsDbContext context, Guid userUuid,
        Guid userRoleUuid)
    {
        await context.Users
            .Where(x => x.Uuid == userUuid)
            .ExecuteUpdateAsync(y => y.SetProperty(z => z.UserRoleUuid,
                z => userRoleUuid));
        await context.SaveChangesAsync();
    }

    public static async Task UpdateUsersPassword(this SsoPasswordChangeRequest passwordChangeRequest,
        JoyModelsDbContext context)
    {
        await context.Users
            .Where(x => x.Uuid == passwordChangeRequest.UserUuid)
            .ExecuteUpdateAsync(y => y.SetProperty(z => z.PasswordHash,
                z => SsoPasswordHasher.Hash(passwordChangeRequest, passwordChangeRequest.NewPassword)));
        await context.SaveChangesAsync();
    }

    public static async Task DeleteAllPendingUserData(JoyModelsDbContext context, Guid userUuid)
    {
        await context.PendingUsers
            .Where(x => x.UserUuid == userUuid)
            .ExecuteDeleteAsync();
        await context.SaveChangesAsync();
    }

    public static async Task DeleteAllUnverifiedUserData(JoyModelsDbContext context, Guid userUuid)
    {
        var numberOfDeletedRows = await context.Users
            .Include(x => x.UserRoleUu)
            .Where(x => x.Uuid == userUuid && x.UserRoleUu.RoleName == nameof(UserRoleEnum.Unverified))
            .ExecuteDeleteAsync();
        await context.SaveChangesAsync();

        if (numberOfDeletedRows == 0)
            throw new KeyNotFoundException(
                $"Unverified user with UUID `{userUuid}` either is verified or does not exist.");
    }

    public static async Task DeleteUserRefreshToken(this SsoLogoutRequest request, JoyModelsDbContext context)
    {
        var numberOfDeletedRows = await context.UserTokens
            .Where(x => x.UserUuid == request.UserUuid && x.RefreshToken == request.UserRefreshToken)
            .ExecuteDeleteAsync();

        if (numberOfDeletedRows == 0)
            throw new KeyNotFoundException(
                "Logout process failed.");

        await context.SaveChangesAsync();
    }

    public static void DeleteUserPictureFolderOnException(Guid userUuid,
        UserImageSettingsDetails userImageSettingsDetails)
    {
        var userPictureFolder = Path.Combine(userImageSettingsDetails.SavePath, "users", userUuid.ToString());
        if (Directory.Exists(userPictureFolder)) Directory.Delete(userPictureFolder, true);
    }

    public static void DeleteUserPictureOnException(string userPictureFileName, Guid userUuid,
        UserImageSettingsDetails userImageSettingsDetails)
    {
        var fullPath = Path.Combine(userImageSettingsDetails.SavePath, "users", userUuid.ToString(),
            userPictureFileName);
        if (File.Exists(fullPath)) File.Delete(fullPath);
    }

    public static void SendEmail(EmailSendUserDetailsRequest emailSendUserDetailsRequest,
        IMessageProducer messageProducer)
    {
        messageProducer.SendMessage("send_email", new EmailSendRequest
        {
            To = emailSendUserDetailsRequest.Email,
            Subject = "JoyModels - Email verification",
            Body =
                $"Your OTP code is: {emailSendUserDetailsRequest.OtpCode} and it lasts until: ${emailSendUserDetailsRequest.OtpExpirationDate}"
        });
    }
}