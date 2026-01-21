using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPost;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPost;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Core;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;

namespace JoyModels.Services.Services.CommunityPost;

public interface ICommunityPostService
{
    Task<CommunityPostResponse> GetByUuid(Guid communityPostUuid);
    Task<PictureResponse> GetCommunityPostPictures(Guid communityPostUuid, string communityPostPictureLocationPath);
    Task<PaginationResponse<CommunityPostResponse>> Search(CommunityPostSearchRequest request);

    Task<PaginationResponse<CommunityPostUserReviewResponse>>
        SearchReviewedUsers(CommunityPostSearchReviewedUsersRequest request);

    Task<CommunityPostResponse> Create(CommunityPostCreateRequest request);
    Task<CommunityPostResponse> Patch(CommunityPostPatchRequest request);
    Task<bool> IsLiked(Guid communityPostUuid);
    Task CreateUserReview(CommunityPostUserReviewCreateRequest request);
    Task<bool> IsDisliked(Guid communityPostUuid);
    Task DeleteUserReview(CommunityPostUserReviewDeleteRequest request);
    Task Delete(Guid communityPostUuid);
}