using System.Transactions;
using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.Jwt;
using JoyModels.Models.DataTransferObjects.RequestTypes.Sso;
using JoyModels.Models.DataTransferObjects.ResponseTypes;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Sso;
using JoyModels.Services.Services.Sso.HelperMethods;
using JoyModels.Services.Validation;
using JoyModels.Services.Validation.Sso;
using UserRoleEnum = JoyModels.Models.Enums.UserRole;

namespace JoyModels.Services.Services.Sso;

public class SsoService : ISsoService
{
    private readonly JoyModelsDbContext _context;
    private readonly IMapper _mapper;
    private readonly JwtClaimDetails _jwtClaimDetails;
    private readonly UserAuthValidation _userAuthValidation;

    public SsoService(JoyModelsDbContext context, IMapper mapper, JwtClaimDetails jwtClaimDetails,
        UserAuthValidation userAuthValidation)
    {
        _context = context;
        _mapper = mapper;
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

        var userRoleEntity = await SsoHelperMethods.GetUserRoleEntity(_context, null, nameof(UserRoleEnum.Unverified));

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
        SsoValidation.ValidateAuthUserRequest(_userAuthValidation.GetAuthUserUuid(), request.UserUuid);
        SsoValidation.ValidateRequestUserUuids(userUuid, request.UserUuid);
        SsoValidation.ValidateOtpCodeValueFormat(request.OtpCode);
        var accessTokenChangeRequest = _mapper.Map<SsoAccessTokenChangeRequest>(request);
        await accessTokenChangeRequest.ValidateUserRefreshToken(_context, _mapper);

        var pendingUserEntity = await SsoHelperMethods
            .GetPendingUserEntity(_context, request.UserUuid);

        SsoValidation.ValidateOtpCodeForUserVerification(pendingUserEntity, request);

        var userRoleEntity = await SsoHelperMethods.GetUserRoleEntity(_context, null, nameof(UserRoleEnum.User));

        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await SsoHelperMethods.DeleteAllPendingUserData(_context, request.UserUuid);
            await SsoHelperMethods.UpdateUsersRole(_context, pendingUserEntity.UserUuid,
                userRoleEntity.Uuid);

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            throw new TransactionException(ex.InnerException!.Message);
        }

        var userEntity = await SsoHelperMethods.GetUserEntity(_context, request.UserUuid, null);
        var ssoAccessTokenChangeResponse =
            SsoHelperMethods.SetCustomValuesSsoAccessTokenChangeResponse(userEntity, _jwtClaimDetails);
        var verifiedUser = _mapper.Map<SsoUserResponse>(userEntity,
            opt => { opt.Items["UserAccessToken"] = ssoAccessTokenChangeResponse.UserAccessToken; });

        return verifiedUser;
    }

    public async Task RequestNewOtpCode(Guid userUuid, SsoNewOtpCodeRequest request)
    {
        SsoValidation.ValidateAuthUserRequest(_userAuthValidation.GetAuthUserUuid(), request.UserUuid);
        SsoValidation.ValidateRequestUserUuids(userUuid, request.UserUuid);

        var pendingUserEntity = _mapper.Map<PendingUser>(request.UserUuid);
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
    }

    public async Task<SsoLoginResponse> Login(SsoLoginRequest request)
    {
        request.ValidateUserLoginRequestArguments();

        var userEntity = await SsoHelperMethods.GetUserEntity(_context, null, request.Nickname);
        request.ValidateUsersPassword(userEntity);

        var ssoLoginResponse = SsoHelperMethods.SetCustomValuesSsoLoginResponse(userEntity, _jwtClaimDetails);

        var userTokenEntity = SsoHelperMethods.SetCustomValuesUserTokenEntity(userEntity, ssoLoginResponse);
        await userTokenEntity.CreateUserToken(_context);

        return ssoLoginResponse;
    }

    public async Task<SsoAccessTokenChangeResponse> RequestAccessTokenChange(Guid userUuid,
        SsoAccessTokenChangeRequest request)
    {
        SsoValidation.ValidateAuthUserRequest(_userAuthValidation.GetAuthUserUuid(),
            request.UserUuid);
        SsoValidation.ValidateRequestUserUuids(userUuid, request.UserUuid);

        await request.ValidateUserRefreshToken(_context, _mapper);

        var userEntity = await SsoHelperMethods.GetUserEntity(_context, request.UserUuid, null);

        var ssoAccessTokenChangeResponse =
            SsoHelperMethods.SetCustomValuesSsoAccessTokenChangeResponse(userEntity, _jwtClaimDetails);

        return ssoAccessTokenChangeResponse;
    }

    public async Task Logout(Guid userUuid, SsoLogoutRequest request)
    {
        SsoValidation.ValidateAuthUserRequest(_userAuthValidation.GetAuthUserUuid(), request.UserUuid);
        SsoValidation.ValidateRequestUserUuids(userUuid, request.UserUuid);

        await request.DeleteUserRefreshToken(_context);
    }

    public async Task RequestPasswordChange(Guid userUuid,
        SsoPasswordChangeRequest request)
    {
        SsoValidation.ValidateAuthUserRequest(_userAuthValidation.GetAuthUserUuid(), request.UserUuid);
        SsoValidation.ValidateRequestUserUuids(userUuid, request.UserUuid);
        request.ValidateUserPasswordChangeRequestArguments();

        await SsoHelperMethods.CheckIfUserExists(_context, request.UserUuid);
        await request.UpdateUsersPassword(_context);
    }

    public async Task SetRole(Guid userUuid, SsoSetRoleRequest request)
    {
        SsoValidation.ValidateRequestUserUuids(userUuid, request.UserUuid);

        var userEntity = await SsoHelperMethods.GetUserEntity(_context, request.UserUuid, null);
        userEntity.CheckIfUserIsUnverified();

        var userRoleEntity =
            await SsoHelperMethods.GetUserRoleEntity(_context, request.DesignatedUserRoleUuid, null);

        userEntity.ValidateIfUserHasSameRole(userRoleEntity);

        await SsoHelperMethods.UpdateUsersRole(_context, request.UserUuid, userRoleEntity.Uuid);
    }

    public async Task Delete(Guid userUuid)
    {
        await SsoHelperMethods.DeleteAllUnverifiedUserData(_context, userUuid);
    }
}