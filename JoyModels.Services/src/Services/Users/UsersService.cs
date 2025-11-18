using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.Users;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
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
        }

        return userResponses;
    }

    public async Task<UsersResponse> FollowAnUser(Guid targetUserUuid)
    {
        await UsersValidation.ValidateUserFollowEndpoint(context, targetUserUuid, userAuthValidation);

        var userFollowerEntity = UsersHelperMethods.CreateUserFollowerObject(targetUserUuid, userAuthValidation);
        await userFollowerEntity.CreateUserFollowerEntity(context);

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

    public async Task Delete(Guid userUuid)
    {
        await UsersHelperMethods.DeleteUserEntity(context, userUuid, userAuthValidation);
    }
}