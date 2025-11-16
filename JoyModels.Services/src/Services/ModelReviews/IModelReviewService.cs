using JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelReviews;

namespace JoyModels.Services.Services.ModelReviews;

public interface IModelReviewService
{
    Task<ModelReviewResponse> GetByUuid(Guid modelReviewUuid);
    Task<ModelReviewResponse> Create(ModelReviewCreateRequest request);
}