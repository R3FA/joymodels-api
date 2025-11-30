using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPost;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPost;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;

namespace JoyModels.Services.Services.CommunityPost;

public interface ICommunityPostService
{
    Task<CommunityPostResponse> GetByUuid(Guid communityPostUuid);
    Task<PaginationResponse<CommunityPostResponse>> Search(CommunityPostSearchRequest request);
    Task<CommunityPostResponse> Create(CommunityPostCreateRequest request);
    Task CreateUserReview(CommunityPostUserReviewCreateRequest request);
    Task DeleteUserReview(CommunityPostUserReviewDeleteRequest request);
    Task Delete(Guid communityPostUuid);
}