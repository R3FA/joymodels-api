using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Models;

public class ModelCreateRequest
{
    [Required] public string Name { get; set; } = string.Empty;

    // TODO: Dodaj logiku za slike isto
    [MaxLength(3000)] [Required] public string Description { get; set; } = string.Empty;
    [Required] public decimal Price { get; set; }
    [Required] public Guid ModelAvailabilityUuid { get; set; }
    [Required] public Guid[] ModelCategoryUuids { get; set; } = [];
}