using JoyModels.Models.DataTransferObjects.RequestTypes.Users;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.UserFollowers;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Services.Services.Users;

public interface IUsersService
{
    Task<UsersResponse> GetByUuid(Guid userUuid);
    Task<PaginationResponse<UsersResponse>> Search(UsersSearchRequest request);
    Task<PaginationResponse<UserFollowingResponse>> SearchFollowingUsers(UserFollowerSearchRequest request);
    Task<PaginationResponse<UserFollowerResponse>> SearchFollowerUsers(UserFollowerSearchRequest request);
    Task<UsersResponse> FollowAnUser(Guid targetUserUuid);
    Task<UsersResponse> Patch(Guid userUuid, UsersPatchRequest request);
    Task<UsersResponse> UnfollowAnUser(Guid targetUserUuid);
    Task Delete(Guid userUuid);
}