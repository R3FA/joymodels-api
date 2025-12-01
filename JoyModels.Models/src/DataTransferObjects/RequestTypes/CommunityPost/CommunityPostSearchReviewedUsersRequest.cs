using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;
using JoyModels.Models.Enums;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPost;

public class CommunityPostSearchReviewedUsersRequest : PaginationRequest
{
    [Required] public Guid CommunityPostUuid { get; set; }
    [Required] public ModelReviewEnum CommunityPostReviewType { get; set; }
}