using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.Users;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Core;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;
using JoyModels.Services.Services.Users.HelperMethods;
using JoyModels.Services.Validation;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

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

    public async Task<PictureResponse> GetUserAvatar(Guid userUuid)
    {
        var userResponse = await GetByUuid(userUuid);

        if (!File.Exists(userResponse.UserPictureLocation))
            throw new KeyNotFoundException("User avatar doesn't exist");

        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(userResponse.UserPictureLocation, out var contentType))
            contentType = "application/octet-stream";

        var fileBytes = await File.ReadAllBytesAsync(userResponse.UserPictureLocation);

        return new PictureResponse
        {
            FileBytes = fileBytes,
            ContentType = contentType,
        };
    }

    public async Task<PaginationResponse<UsersResponse>> Search(UsersSearchRequest request)
    {
        request.ValidateUserSearchArguments();

        var userEntities = await UsersHelperMethods.SearchUserEntities(context, request);

        return mapper.Map<PaginationResponse<UsersResponse>>(userEntities);
    }

    public async Task<PaginationResponse<UsersResponse>> SearchTopArtists(UsersSearchRequest request)
    {
        request.ValidateUserSearchArguments();

        var userEntities = await UsersHelperMethods.SearchTopArtistEntities(context, request);

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

    public async Task<bool> IsFollowingUser(Guid targetUserUuid)
    {
        return await context.UserFollowers
            .AnyAsync(x =>
                x.UserOriginUuid == userAuthValidation.GetUserClaimUuid()
                && x.UserTargetUuid == targetUserUuid);
    }

    public async Task FollowAnUser(Guid targetUserUuid)
    {
        await UsersValidation.ValidateUserFollowEndpoint(context, targetUserUuid, userAuthValidation);

        var userFollowerEntity = UsersHelperMethods.CreateUserFollowerObject(targetUserUuid, userAuthValidation);
        await userFollowerEntity.CreateUserFollowerEntity(context);
    }

    public async Task<UsersResponse> Patch(UsersPatchRequest request)
    {
        userAuthValidation.ValidateUserAuthRequest(request.UserUuid);
        request.ValidateUserPatchArguments();
        await request.ValidateUserPatchArgumentsDuplicatedFields(context);

        var userResponse = await GetByUuid(request.UserUuid);

        await request.PatchUserEntity(context, userResponse, userImageSettingsDetails);

        return await GetByUuid(request.UserUuid);
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