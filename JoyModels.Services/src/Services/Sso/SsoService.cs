using System.Transactions;
using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.Jwt;
using JoyModels.Models.DataTransferObjects.RequestTypes.Email;
using JoyModels.Models.DataTransferObjects.RequestTypes.Sso;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Sso;
using JoyModels.Models.Enums;
using JoyModels.Services.Services.Sso.HelperMethods;
using JoyModels.Services.Validation;
using JoyModels.Utilities.RabbitMQ.MessageProducer;

namespace JoyModels.Services.Services.Sso;

public class SsoService(
    JoyModelsDbContext context,
    IMapper mapper,
    JwtClaimDetails jwtClaimDetails,
    UserAuthValidation userAuthValidation,
    IMessageProducer messageProducer,
    UserImageSettingsDetails userImageSettingsDetails)
    : ISsoService
{
    public async Task<SsoUserResponse> GetByUuid(Guid userUuid)
    {
        var pendingUserEntity = await SsoHelperMethods.GetPendingUserEntity(context, userUuid);
        var pendingUser = mapper.Map<SsoUserResponse>(pendingUserEntity.UserUu);

        return pendingUser;
    }

    public async Task<PaginationResponse<SsoUserResponse>> Search(SsoSearchRequest request)
    {
        request.ValidateUserSearchArguments();

        var pendingUsersEntity = await SsoHelperMethods.SearchPendingUserEntities(context, request);

        var pendingUsers = mapper.Map<PaginationResponse<SsoUserResponse>>(pendingUsersEntity);
        for (var i = 0; i < pendingUsersEntity.Data.Count; i++)
            pendingUsers.Data[i] = mapper.Map<SsoUserResponse>(pendingUsersEntity.Data[i].UserUu);

        return pendingUsers;
    }

    public async Task<SsoUserResponse> Create(SsoUserCreateRequest request)
    {
        request.ValidateUserCreationArguments();
        await request.ValidateUserCreationDuplicatedFields(context);

        var userRoleEntity = await SsoHelperMethods.GetUserRoleEntity(context, null, nameof(UserRoleEnum.Unverified));

        var userEntity = mapper.Map<User>(request);
        userEntity.SetCustomValuesUserEntity(request, userRoleEntity);

        var pendingUserEntity = mapper.Map<PendingUser>(userEntity);
        pendingUserEntity.SetCustomValuesPendingUserEntity();

        var emailSendUserDetailsRequest = mapper.Map<EmailSendUserDetailsRequest>((pendingUserEntity, userEntity));

        userEntity.UserPictureLocation =
            await SsoHelperMethods.SaveUserPicture(request.UserPicture, userImageSettingsDetails,
                userEntity.Uuid);

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await userEntity.CreateUser(context);
            await pendingUserEntity.CreatePendingUser(context);

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            SsoHelperMethods.DeleteUserPictureFolderOnException(userEntity.UserPictureLocation);
            throw new TransactionException(ex.InnerException!.Message);
        }

        SsoHelperMethods.SendEmail(emailSendUserDetailsRequest, messageProducer);

        var updatedUserEntity = mapper.Map<User>(userEntity, opt => { opt.Items["UserRoleEntity"] = userRoleEntity; });
        return mapper.Map<SsoUserResponse>(updatedUserEntity);
    }

    public async Task<SsoUserResponse> Verify(SsoVerifyRequest request)
    {
        userAuthValidation.ValidateUserAuthRequest(request.UserUuid);
        RegularExpressionValidation.ValidateOtpCode(request.OtpCode);
        var accessTokenChangeRequest = mapper.Map<SsoAccessTokenChangeRequest>(request);
        await accessTokenChangeRequest.ValidateUserRefreshToken(context, mapper);

        var pendingUserEntity = await SsoHelperMethods
            .GetPendingUserEntity(context, request.UserUuid);

        SsoValidation.ValidateOtpCodeForUserVerification(pendingUserEntity, request);

        var userRoleEntity = await SsoHelperMethods.GetUserRoleEntity(context, null, nameof(UserRoleEnum.User));

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await SsoHelperMethods.DeleteAllPendingUserData(context, request.UserUuid);
            await SsoHelperMethods.UpdateUsersRole(context, pendingUserEntity.UserUuid,
                userRoleEntity.Uuid);

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            throw new TransactionException(ex.InnerException!.Message);
        }

        var userEntity = await SsoHelperMethods.GetUserEntity(context, request.UserUuid, null);
        var ssoAccessTokenChangeResponse =
            SsoHelperMethods.SetCustomValuesSsoAccessTokenChangeResponse(userEntity, jwtClaimDetails);

        return mapper.Map<SsoUserResponse>(userEntity,
            opt => { opt.Items["UserAccessToken"] = ssoAccessTokenChangeResponse.UserAccessToken; });
    }

    public async Task RequestNewOtpCode(SsoNewOtpCodeRequest request)
    {
        userAuthValidation.ValidateUserAuthRequest(request.UserUuid);

        var userEntity = await SsoHelperMethods.GetUserEntity(context, request.UserUuid, null);

        var pendingUserEntity = mapper.Map<PendingUser>(request.UserUuid);
        pendingUserEntity.SetCustomValuesPendingUserEntity();

        var emailSendUserDetailsRequest =
            mapper.Map<EmailSendUserDetailsRequest>((pendingUserEntity, userEntity));

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await SsoHelperMethods.DeleteAllPendingUserData(context, pendingUserEntity.UserUuid);
            await pendingUserEntity.CreatePendingUser(context);

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            throw new TransactionException(ex.InnerException!.Message);
        }

        SsoHelperMethods.SendEmail(emailSendUserDetailsRequest, messageProducer);
    }

    public async Task<SsoLoginResponse> Login(SsoLoginRequest request)
    {
        request.ValidateUserLoginRequestArguments();

        var userEntity = await SsoHelperMethods.GetUserEntity(context, null, request.Nickname);
        request.ValidateUsersPassword(userEntity);

        var ssoLoginResponse = SsoHelperMethods.SetCustomValuesSsoLoginResponse(userEntity, jwtClaimDetails);

        var userTokenEntity = SsoHelperMethods.SetCustomValuesUserTokenEntity(userEntity, ssoLoginResponse);
        await userTokenEntity.CreateUserToken(context);

        return ssoLoginResponse;
    }

    public async Task<SsoAccessTokenChangeResponse> RequestAccessTokenChange(SsoAccessTokenChangeRequest request)
    {
        userAuthValidation.ValidateUserAuthRequest(request.UserUuid);

        await request.ValidateUserRefreshToken(context, mapper);

        var userEntity = await SsoHelperMethods.GetUserEntity(context, request.UserUuid, null);

        return SsoHelperMethods.SetCustomValuesSsoAccessTokenChangeResponse(userEntity, jwtClaimDetails);
    }

    public async Task Logout(SsoLogoutRequest request)
    {
        userAuthValidation.ValidateUserAuthRequest(request.UserUuid);

        await request.DeleteUserRefreshToken(context);
    }

    public async Task RequestPasswordChange(SsoPasswordChangeRequest request)
    {
        userAuthValidation.ValidateUserAuthRequest(request.UserUuid);
        request.ValidateUserPasswordChangeRequestArguments();

        await SsoHelperMethods.CheckIfUserExists(context, request.UserUuid);
        await request.UpdateUsersPassword(context);
    }

    public async Task SetRole(SsoSetRoleRequest request)
    {
        var userEntity = await SsoHelperMethods.GetUserEntity(context, request.UserUuid, null);
        userEntity.CheckIfUserIsUnverified();

        var userRoleEntity =
            await SsoHelperMethods.GetUserRoleEntity(context, request.DesignatedUserRoleUuid, null);

        userEntity.ValidateIfUserHasSameRole(userRoleEntity);

        await SsoHelperMethods.UpdateUsersRole(context, request.UserUuid, userRoleEntity.Uuid);
    }

    public async Task Delete(Guid userUuid)
    {
        var pendingUser = await GetByUuid(userUuid);

        try
        {
            SsoHelperMethods.DeleteUserPictureFolderOnException(pendingUser.UserPictureLocation);
            await SsoHelperMethods.DeleteAllUnverifiedUserData(context, userUuid);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}