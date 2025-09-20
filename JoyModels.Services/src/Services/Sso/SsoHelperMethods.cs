using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JoyModels.Models.DataTransferObjects.Sso;
using JoyModels.Models.Pagination;
using JoyModels.Models.src.Database.Entities;
using JoyModels.Services.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserRoleEnum = JoyModels.Models.Enums.UserRole;

namespace JoyModels.Services.Services.Sso;

public static class SsoHelperMethods
{
    public static void ValidateUserCreationArguments(this SsoUserCreate request)
    {
        if (!RegularExpressionValidation.IsStringValid(request.FirstName))
            throw new ArgumentException(
                "First name must begin with a capital letter and contain only lowercase letters after.");

        if (request.LastName != null)
        {
            if (!RegularExpressionValidation.IsStringValid(request.LastName))
                throw new ArgumentException(
                    "Last name must begin with a capital letter and contain only lowercase letters after.");
        }

        ValidateNickname(request.Nickname);
        ValidateEmail(request.Email);
        ValidatePassword(request.Password);
    }

    public static async Task ValidateUserCreationDuplicatedFields(this SsoUserCreate request,
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

    public static void ValidateOtpCodeForUserVerification(JoyModelsDbContext context,
        PendingUser pendingUserEntity,
        SsoVerify request)
    {
        if (pendingUserEntity.OtpCode != request.OtpCode)
            throw new ArgumentException("Invalid OTP code.");

        if (DateTime.Now > pendingUserEntity.OtpExpirationDate)
            throw new ArgumentException(
                "Sent OTP Code has expired. Click on resend verification button to regenerate a new OTP code.");
    }

    public static void ValidateUserSearchArguments(this SsoSearch request)
    {
        if (request.Nickname != null)
        {
            ValidateNickname(request.Nickname);
        }

        if (request.Email != null)
        {
            ValidateEmail(request.Email);
        }
    }

    public static async Task ValidateUserRequestPasswordChangeArguments(this SsoRequestPasswordChange request,
        JoyModelsDbContext context)
    {
        await CheckIfUserExists(context, request.UserUuid);

        if (request.NewPassword != request.ConfirmNewPassword)
            throw new ArgumentException("New password and confirm password do not match.");

        ValidatePassword(request.NewPassword);
    }

    public static void ValidateUserLoginArguments(this SsoLogin request)
    {
        ValidateNickname(request.Nickname);
        ValidatePassword(request.Password);
    }

    public static void ValidateUsersPassword(this SsoLogin request, User userEntity)
    {
        var passwordVerificationResult =
            SsoPasswordHasher.Verify(userEntity, userEntity.PasswordHash, request.Password);

        if (passwordVerificationResult is PasswordVerificationResult.Failed)
            throw new ArgumentException("User password is incorrect");
    }

    public static async Task<PendingUser> GetPendingUserEntity(JoyModelsDbContext context, Guid userUuid)
    {
        var pendingUserEntity = await context.PendingUsers
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .Where(x => x.UserUu.UserRoleUu.RoleName == nameof(UserRoleEnum.Unverified))
            .FirstOrDefaultAsync(x => x.UserUuid == userUuid);

        return pendingUserEntity ?? throw new KeyNotFoundException("Pending user with sent values is not found.");
    }

    public static async Task<PaginationBase<PendingUser>> SearchPendingUserEntities(JoyModelsDbContext context,
        SsoSearch ssoSearchDto)
    {
        var baseQuery = context.PendingUsers
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .Where(x => x.UserUu.UserRoleUu.RoleName == nameof(UserRoleEnum.Unverified));

        var filteredQuery = (ssoSearchDto.Nickname, ssoSearchDto.Email) switch
        {
            (not null, null) => baseQuery.Where(x => x.UserUu.NickName == ssoSearchDto.Nickname),
            (null, not null) => baseQuery.Where(x => x.UserUu.Email == ssoSearchDto.Email),
            (not null, not null) => baseQuery.Where(x =>
                x.UserUu.NickName == ssoSearchDto.Nickname &&
                x.UserUu.Email == ssoSearchDto.Email),
            _ => baseQuery
        };

        var pendingUsersEntity = await PaginationBase<PendingUser>.CreateAsync(filteredQuery, ssoSearchDto.PageNumber,
            ssoSearchDto.PageSize);

        return pendingUsersEntity;
    }

    public static async Task<UserRole> GetUserRoleEntity(JoyModelsDbContext context, string roleName)
    {
        var userRoleEntity = await context.UserRoles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.RoleName == roleName);

        return userRoleEntity ??
               throw new KeyNotFoundException($"User role `{roleName}` is not found.");
    }

    public static async Task<User> GetVerifiedUserEntity(JoyModelsDbContext context, Guid? userUuid,
        string? userNickname)
    {
        var baseQuery = context.Users
            .AsNoTracking()
            .Include(x => x.UserRoleUu)
            .Where(x => x.UserRoleUu.RoleName != nameof(UserRoleEnum.Unverified));

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

    public static async Task CheckIfUserIsUnverified(JoyModelsDbContext context, Guid userUuid)
    {
        var userExists = await context.Users
            .AsNoTracking()
            .Include(x => x.UserRoleUu)
            .Where(x => x.UserRoleUu.RoleName == nameof(UserRoleEnum.Unverified))
            .AnyAsync(x => x.Uuid == userUuid);

        if (!userExists)
            throw new KeyNotFoundException(
                $"Unverified user with UUID `{userUuid}` either is verified or does not exist.");
    }

    public static User SetCustomValuesUserEntity(this User userEntity, SsoUserCreate request, UserRole userRoleEntity)
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

    public static async Task CreateUser(this User userEntity, JoyModelsDbContext context)
    {
        await context.Users.AddAsync(userEntity);
        await context.SaveChangesAsync();
    }

    public static async Task CreatePendingUser(this PendingUser pendingUserEntity, JoyModelsDbContext context)
    {
        await context.PendingUsers.AddAsync(pendingUserEntity);
        await context.SaveChangesAsync();
    }

    public static async Task DeleteAllPendingUserData(JoyModelsDbContext context, Guid userUuid)
    {
        await context.PendingUsers
            .Where(x => x.UserUuid == userUuid)
            .ExecuteDeleteAsync();
        await context.SaveChangesAsync();
    }

    public static async Task UpdateUsersRoleAfterVerification(JoyModelsDbContext context, Guid userUuid,
        Guid userRoleUuid)
    {
        await context.Users
            .Where(x => x.Uuid == userUuid)
            .ExecuteUpdateAsync(y => y.SetProperty(z => z.UserRoleUuid,
                z => userRoleUuid));
        await context.SaveChangesAsync();
    }

    public static async Task DeleteAllUnverifiedUserData(JoyModelsDbContext context, Guid userUuid)
    {
        var numberOfDeletedRows = await context.Users
            .Include(x => x.UserRoleUu)
            .Where(x => x.Uuid == userUuid && x.UserRoleUu.RoleName == nameof(UserRoleEnum.Unverified))
            .ExecuteDeleteAsync();

        if (numberOfDeletedRows == 0)
            throw new KeyNotFoundException(
                $"Unverified user with UUID `{userUuid}` either is verified or does not exist.");

        await context.SaveChangesAsync();
    }

    public static async Task UpdateUsersPassword(this SsoRequestPasswordChange request, JoyModelsDbContext context)
    {
        await context.Users
            .Where(x => x.Uuid == request.UserUuid)
            .ExecuteUpdateAsync(y => y.SetProperty(z => z.PasswordHash,
                z => SsoPasswordHasher.Hash(request, request.NewPassword)));
        await context.SaveChangesAsync();
    }

    public static string CreateUserJwtAccessToken(this User user, SsoJwtDetails ssoJwtDetails)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Uuid.ToString()),
            new(ClaimTypes.Name, user.NickName),
            new(ClaimTypes.Role, user.UserRoleUu.RoleName)
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ssoJwtDetails.JwtSigningKey));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512);
        var signingKeyDescriptor = new JwtSecurityToken(
            issuer: ssoJwtDetails.JwtIssuer,
            audience: ssoJwtDetails.JwtAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(signingKeyDescriptor);
    }

    private static string GenerateOtpCode()
    {
        const string otpAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        const int otpCodeLength = 12;
        var chars = new char[otpCodeLength];

        var randomBytes = new byte[otpCodeLength];
        RandomNumberGenerator.Fill(randomBytes);

        for (var i = 0; i < otpCodeLength; i++)
        {
            chars[i] = otpAlphabet[randomBytes[i] % otpAlphabet.Length];
        }

        var otpCode = new string(chars);
        ValidateOtpCodeValueFormat(otpCode);
        return otpCode;
    }

    private static string CreateUserRefreshToken()
    {
        var randomBytes = new byte[64];
        RandomNumberGenerator.Create().GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private static async Task CheckIfUserExists(JoyModelsDbContext context, Guid userUuid)
    {
        var userExists = await context.Users
            .AsNoTracking()
            .Include(x => x.UserRoleUu)
            .Where(x => x.UserRoleUu.RoleName != nameof(UserRoleEnum.Unverified))
            .AnyAsync(x => x.Uuid == userUuid);

        if (!userExists)
            throw new KeyNotFoundException(
                $"User with UUID `{userUuid}` either is unverified or does not exist.");
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