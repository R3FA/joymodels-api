using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostType;

public class CommunityPostTypeSearchRequest : PaginationRequest
{
    [MaxLength(50, ErrorMessage = "PostTypeName cannot exceed 50 characters.")]
    public string? PostTypeName { get; set; }
}