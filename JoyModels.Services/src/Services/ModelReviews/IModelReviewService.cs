using JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;

namespace JoyModels.Services.Services.ModelReviews;

public interface IModelReviewService
{
    Task<ModelReviewResponse> GetByUuid(Guid modelReviewUuid);
    Task<PaginationResponse<ModelReviewResponse>> Search(ModelReviewSearchRequest request);
    Task<bool> HasUserReviewed(Guid modelUuid);
    Task<ModelCalculatedReviewsResponse> CalculateReviews(Guid modelUuid);
    Task<ModelReviewResponse> Create(ModelReviewCreateRequest request);
    Task<ModelReviewResponse> Patch(ModelReviewPatchRequest request);
    Task Delete(Guid modelReviewUuid);
}