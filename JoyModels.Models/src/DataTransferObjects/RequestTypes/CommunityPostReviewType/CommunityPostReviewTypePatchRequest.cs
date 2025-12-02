using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostReviewType;

public class CommunityPostReviewTypePatchRequest
{
    [Required] public Guid CommunityPostReviewTypeUuid { get; set; }

    [Required, MaxLength(50, ErrorMessage = "CommunityPostReviewTypeName cannot exceed 50 characters.")]
    public string CommunityPostReviewTypeName { get; set; } = string.Empty;
}