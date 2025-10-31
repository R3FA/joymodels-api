using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.ModelAvailability;

public class ModelAvailabilityPatchRequest
{
    [Required] public Guid Uuid { get; set; }
    [Required] public string AvailabilityName { get; set; } = string.Empty;
}