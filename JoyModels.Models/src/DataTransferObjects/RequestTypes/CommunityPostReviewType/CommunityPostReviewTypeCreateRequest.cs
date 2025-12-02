using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostReviewType;

public class CommunityPostReviewTypeCreateRequest
{
    [Required, MaxLength(50, ErrorMessage = "CommunityPostReviewTypeName cannot exceed 50 characters.")]
    public string CommunityPostReviewTypeName { get; set; } = string.Empty;
}