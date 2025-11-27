using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.ModelFaqSection;

public class ModelFaqSectionDeleteRequest
{
    [Required] public Guid ModelFaqSectionUuid { get; set; }
    [Required] public Guid ModelUuid { get; set; }
}