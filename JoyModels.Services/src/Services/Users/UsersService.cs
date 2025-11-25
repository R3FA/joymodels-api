using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.Users;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.UserFollowers;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;
using JoyModels.Services.Services.Users.HelperMethods;
using JoyModels.Services.Validation;

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

        return mapper.Map<UsersResponse>(userEntity);
    }

    public async Task<PaginationResponse<UsersResponse>> Search(UsersSearchRequest request)
    {
        request.ValidateUserSearchArguments();

        var userEntities = await UsersHelperMethods.SearchUserEntities(context, request);

        return mapper.Map<PaginationResponse<UsersResponse>>(userEntities);
    }

    public async Task<PaginationResponse<UserFollowingResponse>> SearchFollowingUsers(
        UserFollowerSearchRequest request)
    {
        request.ValidateUserSearchFollowingUsersArguments();

        var userFollowingEntities = await UsersHelperMethods.SearchFollowingUsers(context, request);

        return mapper.Map<PaginationResponse<UserFollowingResponse>>(userFollowingEntities);
    }

    public async Task<PaginationResponse<UserFollowerResponse>> SearchFollowerUsers(UserFollowerSearchRequest request)
    {
        request.ValidateUserSearchFollowingUsersArguments();

        var userFollowerEntities = await UsersHelperMethods.SearchFollowerUsers(context, request);

        return mapper.Map<PaginationResponse<UserFollowerResponse>>(userFollowerEntities);
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

    public async Task FollowAnUser(Guid targetUserUuid)
    {
        await UsersValidation.ValidateUserFollowEndpoint(context, targetUserUuid, userAuthValidation);

        var userFollowerEntity = UsersHelperMethods.CreateUserFollowerObject(targetUserUuid, userAuthValidation);
        await userFollowerEntity.CreateUserFollowerEntity(context);
    }

    public async Task<UsersResponse> Patch(Guid userUuid, UsersPatchRequest request)
    {
        userAuthValidation.ValidateUserAuthRequest(userUuid);
        GlobalValidation.ValidateRequestUuids(userUuid, request.UserUuid);
        request.ValidateUserPatchArguments();
        await request.ValidateUserPatchArgumentsDuplicatedFields(context);

        var userResponse = await GetByUuid(userUuid);

        await request.PatchUserEntity(context, userResponse, userImageSettingsDetails);

        return await GetByUuid(userUuid);
    }

    public async Task UnfollowAnUser(Guid targetUserUuid)
    {
        await UsersValidation.ValidateUserUnfollowEndpoint(context, targetUserUuid, userAuthValidation);

        await UsersHelperMethods.DeleteUserFollowerEntity(context, targetUserUuid, userAuthValidation);
    }

    public async Task Delete(Guid userUuid)
    {
        await UsersHelperMethods.DeleteUserEntity(context, userUuid, userAuthValidation);
    }
}