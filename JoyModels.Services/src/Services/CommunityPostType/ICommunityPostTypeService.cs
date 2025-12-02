using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPostType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;

namespace JoyModels.Services.Services.CommunityPostType;

public interface ICommunityPostTypeService
{
    Task<CommunityPostTypeResponse> GetByUuid(Guid communityPostTypeUuid);
    Task<PaginationResponse<CommunityPostTypeResponse>> Search(CommunityPostTypeSearchRequest request);
    Task<CommunityPostTypeResponse> Create(CommunityPostTypeCreateRequest request);
    Task<CommunityPostTypeResponse> Patch(CommunityPostTypePatchRequest request);
    Task Delete(Guid communityPostTypeUuid);
}