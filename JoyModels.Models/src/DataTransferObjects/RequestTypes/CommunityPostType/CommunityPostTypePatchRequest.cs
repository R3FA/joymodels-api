using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostType;

public class CommunityPostTypePatchRequest
{
    [Required] public Guid PostTypeUuid { get; set; }

    [Required, MaxLength(50, ErrorMessage = "PostTypeName cannot exceed 50 characters.")]
    public string PostTypeName { get; set; } = string.Empty;
}