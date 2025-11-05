using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Models;

public class ModelCreateRequest
{
    [Required] public string Name { get; set; } = string.Empty;
    [Required, MinLength(1), MaxLength(8)] public List<IFormFile> Pictures { get; set; } = null!;
    [MaxLength(3000)] [Required] public string Description { get; set; } = string.Empty;
    [Required] public decimal Price { get; set; }
    [Required] public Guid ModelAvailabilityUuid { get; set; }
    [Required, MinLength(1), MaxLength(5)] public List<Guid> ModelCategoryUuids { get; set; } = null!;
}