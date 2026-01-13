using JoyModels.Models.DataTransferObjects.RequestTypes.Users;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Core;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Services.Services.Users;

public interface IUsersService
{
    Task<UsersResponse> GetByUuid(Guid userUuid);
    Task<PictureResponse> GetUserAvatar(Guid userUuid);
    Task<PaginationResponse<UsersResponse>> Search(UsersSearchRequest request);
    Task<PaginationResponse<UsersResponse>> SearchTopArtists(UsersSearchRequest request);
    Task<PaginationResponse<UserFollowingResponse>> SearchFollowingUsers(UserFollowerSearchRequest request);
    Task<PaginationResponse<UserFollowerResponse>> SearchFollowerUsers(UserFollowerSearchRequest request);
    Task<PaginationResponse<UserModelLikesSearchResponse>> SearchUserModelLikes(UserModelLikesSearchRequest request);
    Task FollowAnUser(Guid targetUserUuid);
    Task<UsersResponse> Patch(UsersPatchRequest request);
    Task UnfollowAnUser(Guid targetUserUuid);
    Task Delete(Guid userUuid);
}