using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPost;

public class CommunityPostSearchUserLikedPosts : PaginationRequest
{
    [Required] public Guid UserUuid { get; set; }
}