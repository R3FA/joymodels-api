using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostReviewType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPostReviewType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;

namespace JoyModels.Services.Services.CommunityPostReviewType;

public interface ICommunityPostReviewTypeService
{
    Task<CommunityPostReviewTypeResponse> GetByUuid(Guid communityPostReviewTypeUuid);
    Task<PaginationResponse<CommunityPostReviewTypeResponse>> Search(CommunityPostReviewTypeSearchRequest request);
    Task<CommunityPostReviewTypeResponse> Create(CommunityPostReviewTypeCreateRequest request);
    Task<CommunityPostReviewTypeResponse> Patch(CommunityPostReviewTypePatchRequest request);
    Task Delete(Guid communityPostReviewTypeUuid);
}