using System.Transactions;
using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.Jwt;
using JoyModels.Models.DataTransferObjects.RequestTypes.Sso;
using JoyModels.Models.DataTransferObjects.ResponseTypes;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Sso;
using JoyModels.Services.Validation;
using Microsoft.AspNetCore.Http;
using UserRoleEnum = JoyModels.Models.Enums.UserRole;

namespace JoyModels.Services.Services.Sso;

public class SsoService : ISsoService
{
    private readonly JoyModelsDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContext;
    private readonly JwtClaimDetails _jwtClaimDetails;
    private readonly UserAuthValidation _userAuthValidation;

    public SsoService(JoyModelsDbContext context, IMapper mapper, IHttpContextAccessor httpContext,
        JwtClaimDetails jwtClaimDetails, UserAuthValidation userAuthValidation)
    {
        _context = context;
        _mapper = mapper;
        _httpContext = httpContext;
        _jwtClaimDetails = jwtClaimDetails;
        _userAuthValidation = userAuthValidation;
    }

    public async Task<SsoResponse> GetByUuid(Guid userUuid)
    {
        var pendingUserEntity = await SsoHelperMethods.GetPendingUserEntity(_context, userUuid);
        var pendingUser = _mapper.Map<SsoResponse>(pendingUserEntity);

        return pendingUser;
    }

    public async Task<PaginationResponse<SsoResponse>> Search(SsoSearchRequest request)
    {
        request.ValidateUserSearchArguments();

        var pendingUsersEntity = await SsoHelperMethods.SearchPendingUserEntities(_context, request);
        var pendingUsers = _mapper.Map<PaginationResponse<SsoResponse>>(pendingUsersEntity);

        return pendingUsers;
    }

    public async Task<SsoUserResponse> Create(SsoUserCreateRequest request)
    {
        request.ValidateUserCreationArguments();
        await request.ValidateUserCreationDuplicatedFields(_context);

        var userRoleEntity = await SsoHelperMethods.GetUserRoleEntity(_context, nameof(UserRoleEnum.Unverified));

        var userEntity = _mapper.Map<User>(request);
        userEntity.SetCustomValuesUserEntity(request, userRoleEntity);

        var pendingUserEntity = _mapper.Map<PendingUser>(userEntity);
        pendingUserEntity.SetCustomValuesPendingUserEntity();

        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await userEntity.CreateUser(_context);
            await pendingUserEntity.CreatePendingUser(_context);

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            throw new TransactionException(ex.InnerException!.Message);
        }

        var updatedUserEntity = _mapper.Map<User>(userEntity, opt => { opt.Items["UserRole"] = userRoleEntity; });
        return _mapper.Map<SsoUserResponse>(updatedUserEntity);
    }

    public async Task<SsoUserResponse> Verify(Guid userUuid, SsoVerifyRequest request)
    {
        SsoHelperMethods.ValidateRequestUserUuids(userUuid, request.UserUuid);
        SsoHelperMethods.ValidateAuthUserRequest(_userAuthValidation.GetAuthUserUuid(), request.UserUuid);

        SsoHelperMethods.ValidateOtpCodeValueFormat(request.OtpCode);

        var pendingUserEntity = await SsoHelperMethods
            .GetPendingUserEntity(_context, request.UserUuid);

        SsoHelperMethods.ValidateOtpCodeForUserVerification(pendingUserEntity, request);

        var userRoleEntity = await SsoHelperMethods.GetUserRoleEntity(_context, nameof(UserRoleEnum.User));

        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await SsoHelperMethods.DeleteAllPendingUserData(_context, request.UserUuid);
            await SsoHelperMethods.UpdateUsersRoleAfterVerification(_context, pendingUserEntity.UserUuid,
                userRoleEntity.Uuid);

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            throw new TransactionException(ex.InnerException!.Message);
        }

        var userEntity = await SsoHelperMethods.GetUserEntity(_context, request.UserUuid, null);
        var verifiedUser = _mapper.Map<SsoUserResponse>(userEntity);

        return verifiedUser;
    }

    public async Task<SuccessResponse> RequestNewOtpCode(Guid userUuid, SsoNewOtpCodeRequest newOtpCodeRequest)
    {
        SsoHelperMethods.ValidateRequestUserUuids(userUuid, newOtpCodeRequest.UserUuid);
        SsoHelperMethods.ValidateAuthUserRequest(_userAuthValidation.GetAuthUserUuid(), newOtpCodeRequest.UserUuid);

        await SsoHelperMethods.CheckIfUserIsUnverified(_context, newOtpCodeRequest.UserUuid);

        var pendingUserEntity = _mapper.Map<PendingUser>(newOtpCodeRequest.UserUuid);
        pendingUserEntity.SetCustomValuesPendingUserEntity();

        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await SsoHelperMethods.DeleteAllPendingUserData(_context, pendingUserEntity.UserUuid);
            await pendingUserEntity.CreatePendingUser(_context);

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            throw new TransactionException(ex.InnerException!.Message);
        }

        return new SuccessResponse()
        {
            Type = "Success",
            Title = "Created",
            Detail = "Otp code has been generated and sent to your email.",
            Status = StatusCodes.Status200OK.ToString(),
            Instance = _httpContext.HttpContext.Request.Path.ToString()
        };
    }

    public async Task<SsoLoginResponse> Login(SsoLoginRequest request)
    {
        request.ValidateUserLoginArguments();

        var userEntity = await SsoHelperMethods.GetUserEntity(_context, null, request.Nickname);
        request.ValidateUsersPassword(userEntity);

        var ssoLoginResponse = SsoHelperMethods.SetCustomValuesSsoLoginResponse(userEntity, _jwtClaimDetails);

        var userTokenEntity = SsoHelperMethods.SetCustomValuesUserTokenEntity(userEntity, ssoLoginResponse);
        await userTokenEntity.CreateUserToken(_context);

        return ssoLoginResponse;
    }

    public async Task<SsoAccessTokenChangeResponse> RequestAccessTokenChange(Guid userUuid,
        SsoAccessTokenChangeRequest accessTokenChangeRequest)
    {
        SsoHelperMethods.ValidateRequestUserUuids(userUuid, accessTokenChangeRequest.UserUuid);
        SsoHelperMethods.ValidateAuthUserRequest(_userAuthValidation.GetAuthUserUuid(),
            accessTokenChangeRequest.UserUuid);

        await accessTokenChangeRequest.ValidateUserRefreshToken(_context, _mapper);

        var userEntity = await SsoHelperMethods.GetUserEntity(_context, accessTokenChangeRequest.UserUuid, null);

        var ssoRequestAccessTokenChangeRequest =
            SsoHelperMethods.SetCustomValuesSsoRequestAccessTokenChangeResponse(userEntity, _jwtClaimDetails);

        return ssoRequestAccessTokenChangeRequest;
    }

    public async Task<SuccessResponse> Logout(Guid userUuid, SsoLogoutRequest request)
    {
        SsoHelperMethods.ValidateRequestUserUuids(userUuid, request.UserUuid);
        SsoHelperMethods.ValidateAuthUserRequest(_userAuthValidation.GetAuthUserUuid(), request.UserUuid);

        await request.DeleteUserRefreshToken(_context);

        return new SuccessResponse
        {
            Type = "Success",
            Title = "Deleted",
            Detail = "You have successfully logged out of your account.",
            Status = StatusCodes.Status200OK.ToString(),
            Instance = _httpContext.HttpContext.Request.Path.ToString()
        };
    }

    public async Task<SuccessResponse> RequestPasswordChange(Guid userUuid,
        SsoPasswordChangeRequest passwordChangeRequest)
    {
        SsoHelperMethods.ValidateRequestUserUuids(userUuid, passwordChangeRequest.UserUuid);
        SsoHelperMethods.ValidateAuthUserRequest(_userAuthValidation.GetAuthUserUuid(), passwordChangeRequest.UserUuid);

        await passwordChangeRequest.ValidateUserRequestPasswordChangeArguments(_context);
        await passwordChangeRequest.UpdateUsersPassword(_context);

        return new SuccessResponse
        {
            Type = "Success",
            Title = "Patched",
            Detail = "You have successfully changed your password.",
            Status = StatusCodes.Status200OK.ToString(),
            Instance = _httpContext.HttpContext.Request.Path.ToString()
        };
    }

    public async Task<SuccessResponse> Delete(Guid userUuid)
    {
        await SsoHelperMethods.DeleteAllUnverifiedUserData(_context, userUuid);

        return new SuccessResponse
        {
            Type = "Success",
            Title = "Deleted",
            Detail = $"Unverified user with UUID `{userUuid}` has been successfully deleted from our database.",
            Status = StatusCodes.Status200OK.ToString(),
            Instance = _httpContext.HttpContext.Request.Path.ToString()
        };
    }
}