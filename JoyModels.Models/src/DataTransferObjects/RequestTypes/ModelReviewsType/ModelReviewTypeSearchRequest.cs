using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviewsType;

public class ModelReviewTypeSearchRequest : PaginationRequest
{
    [MaxLength(50, ErrorMessage = "ModelReviewTypeName cannot exceed 50 characters.")]
    public string? ModelReviewTypeName { get; set; }
}