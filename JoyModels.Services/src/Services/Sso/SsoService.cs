using System.Transactions;
using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.CustomResponseTypes;
using JoyModels.Models.DataTransferObjects.Sso;
using Microsoft.AspNetCore.Http;
using UserRoleEnum = JoyModels.Models.Enums.UserRole;

namespace JoyModels.Services.Services.Sso;

public class SsoService : ISsoService
{
    private readonly JoyModelsDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContext;
    private readonly SsoJwtDetails _ssoJwtDetails;

    public SsoService(JoyModelsDbContext context, IMapper mapper, IHttpContextAccessor httpContext,
        SsoJwtDetails ssoJwtDetails)
    {
        _context = context;
        _mapper = mapper;
        _httpContext = httpContext;
        _ssoJwtDetails = ssoJwtDetails;
    }

    public async Task<SsoReturn> GetByUuid(SsoGetByUuid request)
    {
        var pendingUserEntity = await SsoHelperMethods.GetPendingUserEntity(_context, request.UserUuid);
        var pendingUser = _mapper.Map<SsoReturn>(pendingUserEntity);

        return pendingUser;
    }

    public async Task<PaginationResponse<SsoReturn>> Search(SsoSearch request)
    {
        request.ValidateUserSearchArguments();

        var pendingUsersEntity = await SsoHelperMethods.SearchPendingUserEntities(_context, request);
        var pendingUsers = _mapper.Map<PaginationResponse<SsoReturn>>(pendingUsersEntity);

        return pendingUsers;
    }

    public async Task<SsoUserGet> Create(SsoUserCreate request)
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
        return _mapper.Map<SsoUserGet>(updatedUserEntity);
    }

    public async Task<SsoUserGet> Verify(SsoVerify request)
    {
        SsoHelperMethods.ValidateOtpCodeValueFormat(request.OtpCode);

        var pendingUserEntity = await SsoHelperMethods
            .GetPendingUserEntity(_context, request.UserUuid);

        SsoHelperMethods.ValidateOtpCodeForUserVerification(_context, pendingUserEntity, request);

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

        var userEntity = await SsoHelperMethods.GetVerifiedUserEntity(_context, request.UserUuid, null);
        var verifiedUser = _mapper.Map<SsoUserGet>(userEntity);

        return verifiedUser;
    }

    public async Task<SuccessResponse> RequestNewOtpCode(SsoRequestNewOtpCode request)
    {
        await SsoHelperMethods.CheckIfUserIsUnverified(_context, request.UserUuid);

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

        return new SuccessResponse()
        {
            Type = "Success",
            Title = "Created",
            Detail = "Otp code has been generated and sent to your email.",
            Status = StatusCodes.Status200OK.ToString(),
            Instance = _httpContext.HttpContext.Request.Path.ToString()
        };
    }

    public async Task<SsoLoginResponse> Login(SsoLogin request)
    {
        request.ValidateUserLoginArguments();

        var userEntity = await SsoHelperMethods.GetVerifiedUserEntity(_context, null, request.Nickname);
        request.ValidateUsersPassword(userEntity);

        var ssoLoginResponse = SsoHelperMethods.SetCustomValuesSsoLoginResponse(userEntity, _ssoJwtDetails);

        var userTokenEntity = SsoHelperMethods.SetCustomValuesUserTokenEntity(userEntity, ssoLoginResponse);
        await userTokenEntity.CreateUserToken(_context);

        return ssoLoginResponse;
    }

    public async Task<SuccessResponse> Logout(SsoLogoutRequest request)
    {
        await request.DeleteUserRefreshToken(_context);

        return new SuccessResponse()
        {
            Type = "Success",
            Title = "Deleted",
            Detail = "You have successfully logged out of your account.",
            Status = StatusCodes.Status200OK.ToString(),
            Instance = _httpContext.HttpContext.Request.Path.ToString()
        };
    }

    public async Task<SuccessResponse> RequestPasswordChange(SsoRequestPasswordChange request)
    {
        await request.ValidateUserRequestPasswordChangeArguments(_context);
        await request.UpdateUsersPassword(_context);

        return new SuccessResponse()
        {
            Type = "Success",
            Title = "Patched",
            Detail = "You have successfully changed your password.",
            Status = StatusCodes.Status200OK.ToString(),
            Instance = _httpContext.HttpContext.Request.Path.ToString()
        };
    }

    public async Task<SuccessResponse> Delete(SsoDelete request)
    {
        await SsoHelperMethods.DeleteAllUnverifiedUserData(_context, request.UserUuid);

        return new SuccessResponse()
        {
            Type = "Success",
            Title = "Deleted",
            Detail = $"Unverified user with UUID `{request.UserUuid}` has been successfully deleted from our database.",
            Status = StatusCodes.Status200OK.ToString(),
            Instance = _httpContext.HttpContext.Request.Path.ToString()
        };
    }
}