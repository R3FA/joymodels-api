using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostReviewType;

public class CommunityPostReviewTypeSearchRequest : PaginationRequest
{
    [MaxLength(50, ErrorMessage = "CommunityPostReviewTypeName cannot exceed 50 characters.")]
    public string? CommunityPostReviewTypeName { get; set; }
}