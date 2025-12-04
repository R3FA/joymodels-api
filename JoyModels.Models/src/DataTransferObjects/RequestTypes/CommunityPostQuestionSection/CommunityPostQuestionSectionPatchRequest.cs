using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostQuestionSection;

public class CommunityPostQuestionSectionPatchRequest
{
    [Required] public Guid CommunityPostQuestionSectionUuid { get; set; }

    [Required, MaxLength(5000, ErrorMessage = "MessageText cannot exceed 5000 characters.")]
    public string MessageText { get; set; } = string.Empty;
}