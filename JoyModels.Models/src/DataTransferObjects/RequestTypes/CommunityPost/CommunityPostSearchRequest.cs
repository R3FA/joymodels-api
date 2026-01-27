using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPost;

public class CommunityPostSearchRequest : PaginationRequest
{
    [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
    public string? Title { get; set; }

    public Guid? UserUuid { get; set; }
}