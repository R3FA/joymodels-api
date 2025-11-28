using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPost;

public class CommunityPostUserReviewCreateRequest
{
    [Required] public Guid CommunityPostUuid { get; set; }
    [Required] public Guid ReviewTypeUuid { get; set; }
}