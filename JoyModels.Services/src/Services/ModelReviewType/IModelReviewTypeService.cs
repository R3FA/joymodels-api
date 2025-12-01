using JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviewsType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelReviewsType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;

namespace JoyModels.Services.Services.ModelReviewType;

public interface IModelReviewTypeService
{
    Task<ModelReviewTypeResponse> GetByUuid(Guid modelReviewTypeUuid);
    Task<PaginationResponse<ModelReviewTypeResponse>> Search(ModelReviewTypeSearchRequest request);
    Task<ModelReviewTypeResponse> Create(ModelReviewTypeCreateRequest request);
    Task<ModelReviewTypeResponse> Patch(ModelReviewTypePatchRequest request);
    Task Delete(Guid modelReviewTypeUuid);
}