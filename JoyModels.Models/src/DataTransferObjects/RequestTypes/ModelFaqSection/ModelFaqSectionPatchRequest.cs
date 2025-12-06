using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.ModelFaqSection;

public class ModelFaqSectionPatchRequest
{
    [Required] public Guid ModelFaqSectionUuid { get; set; }
    [Required] public Guid ModelUuid { get; set; }

    [Required, MaxLength(5000, ErrorMessage = "MessageText cannot exceed 5000 characters.")]
    public string MessageText { get; set; } = string.Empty;
}