using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.ModelFaqSection;

public class ModelFaqSectionCreateAnswerRequest
{
    [Required] public Guid ModelUuid { get; set; }

    [Required] public Guid ParentMessageUuid { get; set; }

    [Required, MaxLength(1500, ErrorMessage = "MessageText cannot exceed 1500 characters.")]
    public string MessageText { get; set; } = string.Empty;
}