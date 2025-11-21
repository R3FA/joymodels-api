using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.Users;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.UserFollowers;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;
using JoyModels.Services.Services.Users.HelperMethods;
using JoyModels.Services.Validation;
using JoyModels.Services.Validation.Users;

namespace JoyModels.Services.Services.Users;

public class UsersService(
    JoyModelsDbContext context,
    IMapper mapper,
    UserAuthValidation userAuthValidation,
    UserImageSettingsDetails userImageSettingsDetails)
    : IUsersService
{
    public async Task<UsersResponse> GetByUuid(Guid userUuid)
    {
        var userEntity = await UsersHelperMethods.GetUserEntity(context, userUuid);

        var userResponse = mapper.Map<UsersResponse>(userEntity);
        userResponse.UserFollowing = await UsersHelperMethods.GetUserFollowing(context, userUuid);
        userResponse.UserFollowers = await UsersHelperMethods.GetUserFollowers(context, userUuid);
        userResponse.UserModelLikes = await UsersHelperMethods.GetUserModelLikes(context, userUuid);

        return userResponse;
    }

    public async Task<PaginationResponse<UsersResponse>> Search(UsersSearchRequest request)
    {
        request.ValidateUserSearchArguments();

        var userEntities = await UsersHelperMethods.SearchUserEntities(context, request);

        var userResponses = mapper.Map<PaginationResponse<UsersResponse>>(userEntities);
        foreach (var userResponse in userResponses.Data)
        {
            userResponse.UserFollowing = await UsersHelperMethods.GetUserFollowing(context, userResponse.Uuid);
            userResponse.UserFollowers = await UsersHelperMethods.GetUserFollowers(context, userResponse.Uuid);
            userResponse.UserModelLikes = await UsersHelperMethods.GetUserModelLikes(context, userResponse.Uuid);
        }

        return userResponses;
    }

    public async Task<PaginationResponse<UserFollowingResponse>> SearchFollowingUsers(
        UserFollowerSearchRequest request)
    {
        request.ValidateUserSearchFollowingUsersArguments();

        var userFollowingEntities = await UsersHelperMethods.SearchFollowingUsers(context, request);
        var userFollowingResponses = mapper.Map<PaginationResponse<UserFollowingResponse>>(userFollowingEntities);

        foreach (var userFollowingResponse in userFollowingResponses.Data)
        {
            userFollowingResponse.TargetUser.UserFollowing =
                await UsersHelperMethods.GetUserFollowing(context, userFollowingResponse.TargetUser.Uuid);
            userFollowingResponse.TargetUser.UserFollowers =
                await UsersHelperMethods.GetUserFollowers(context, userFollowingResponse.TargetUser.Uuid);
            userFollowingResponse.TargetUser.UserModelLikes =
                await UsersHelperMethods.GetUserModelLikes(context, userFollowingResponse.TargetUser.Uuid);
        }

        return userFollowingResponses;
    }

    public async Task<PaginationResponse<UserFollowerResponse>> SearchFollowerUsers(UserFollowerSearchRequest request)
    {
        request.ValidateUserSearchFollowingUsersArguments();

        var userFollowerEntities = await UsersHelperMethods.SearchFollowerUsers(context, request);
        var userFollowerResponses = mapper.Map<PaginationResponse<UserFollowerResponse>>(userFollowerEntities);

        foreach (var userFollowersResponse in userFollowerResponses.Data)
        {
            userFollowersResponse.OriginUser.UserFollowing =
                await UsersHelperMethods.GetUserFollowing(context, userFollowersResponse.OriginUser.Uuid);
            userFollowersResponse.OriginUser.UserFollowers =
                await UsersHelperMethods.GetUserFollowers(context, userFollowersResponse.OriginUser.Uuid);
            userFollowersResponse.OriginUser.UserModelLikes =
                await UsersHelperMethods.GetUserModelLikes(context, userFollowersResponse.OriginUser.Uuid);
        }

        return userFollowerResponses;
    }

    public async Task<PaginationResponse<UserModelLikesSearchResponse>> SearchUserModelLikes(
        UserModelLikesSearchRequest request)
    {
        request.ValidateUserModelLikesSearchArguments();

        var userModelLikeEntities = await UsersHelperMethods.SearchUserModelLikes(context, request);
        var userModelLikeResponses =
            mapper.Map<PaginationResponse<UserModelLikesSearchResponse>>(userModelLikeEntities);

        return userModelLikeResponses;
    }

    public async Task<UsersResponse> FollowAnUser(Guid targetUserUuid)
    {
        await UsersValidation.ValidateUserFollowEndpoint(context, targetUserUuid, userAuthValidation);

        var userFollowerEntity = UsersHelperMethods.CreateUserFollowerObject(targetUserUuid, userAuthValidation);
        await userFollowerEntity.CreateUserFollowerEntity(context);

        return await GetByUuid(userAuthValidation.GetUserClaimUuid());
    }

    public async Task<UsersResponse> ModelLike(Guid modelUuid)
    {
        await UsersValidation.ValidateModelLikeEndpoint(context, modelUuid, userAuthValidation);

        var userModelLikeEntity = UsersHelperMethods.CreateUserModelLikeObject(modelUuid, userAuthValidation);
        await userModelLikeEntity.CreateUserModelLikeEntity(context);

        return await GetByUuid(userAuthValidation.GetUserClaimUuid());
    }

    public async Task<UsersResponse> Patch(Guid userUuid, UsersPatchRequest request)
    {
        userAuthValidation.ValidateUserAuthRequest(userUuid);
        userAuthValidation.ValidateRequestUuids(userUuid, request.UserUuid);
        request.ValidateUserPatchArguments();
        await request.ValidateUserPatchArgumentsDuplicatedFields(context);

        var userResponse = await GetByUuid(userUuid);

        await request.PatchUserEntity(context, userResponse, userImageSettingsDetails);

        return await GetByUuid(userUuid);
    }

    public async Task<UsersResponse> UnfollowAnUser(Guid targetUserUuid)
    {
        await UsersValidation.ValidateUserUnfollowEndpoint(context, targetUserUuid, userAuthValidation);

        await UsersHelperMethods.DeleteUserFollowerEntity(context, targetUserUuid, userAuthValidation);

        return await GetByUuid(userAuthValidation.GetUserClaimUuid());
    }

    public async Task<UsersResponse> ModelUnlike(Guid modelUuid)
    {
        await UsersValidation.ValidateModelUnlikeEndpoint(context, modelUuid, userAuthValidation);

        await UsersHelperMethods.DeleteUserModelLikeEntity(context, modelUuid, userAuthValidation);

        return await GetByUuid(userAuthValidation.GetUserClaimUuid());
    }

    public async Task Delete(Guid userUuid)
    {
        await UsersHelperMethods.DeleteUserEntity(context, userUuid, userAuthValidation);
    }
}