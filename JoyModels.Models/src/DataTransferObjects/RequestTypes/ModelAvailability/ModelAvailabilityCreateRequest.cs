using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.ModelAvailability;

public class ModelAvailabilityCreateRequest
{
    [Required] public string AvailabilityName { get; set; } = string.Empty;
}