using JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;

namespace JoyModels.Services.Services.ModelReviews;

public interface IModelReviewService
{
    Task<ModelReviewResponse> GetByUuid(Guid modelReviewUuid);
    Task<PaginationResponse<ModelReviewResponse>> Search(ModelReviewSearchRequest request);
    Task<ModelReviewResponse> Create(ModelReviewCreateRequest request);
    Task Delete(Guid modelReviewUuid);
}