using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.ModelAvailability;

public class ModelAvailabilityCreateRequest
{
    [Required, MaxLength(50, ErrorMessage = "AvailabilityName cannot exceed 50 characters.")]
    public string AvailabilityName { get; set; } = string.Empty;
}