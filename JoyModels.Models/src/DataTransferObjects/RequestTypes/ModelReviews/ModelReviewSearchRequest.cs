using System.ComponentModel;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;
using JoyModels.Models.Enums;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviews;

public class ModelReviewSearchRequest : PaginationRequest
{
    [DefaultValue(ModelReview.All)] public ModelReview ModelReviewType { get; set; }
}