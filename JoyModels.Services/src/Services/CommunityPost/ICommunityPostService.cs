using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPost;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPost;

namespace JoyModels.Services.Services.CommunityPost;

public interface ICommunityPostService
{
    Task<CommunityPostResponse> GetByUuid(Guid communityPostUuid);
    Task<CommunityPostResponse> Create(CommunityPostCreateRequest request);
    Task CreateUserReview(CommunityPostUserReviewCreateRequest request);
}