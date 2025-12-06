using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Models;

public class ModelCreateRequest
{
    [Required, MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(8)] public List<IFormFile> Pictures { get; set; } = null!;

    [MaxLength(5000, ErrorMessage = "Description cannot exceed 5000 characters.")]
    [Required]
    public string Description { get; set; } = string.Empty;

    [Required] public decimal Price { get; set; }
    [Required] public Guid ModelAvailabilityUuid { get; set; }
    [Required, MaxLength(5)] public List<Guid> ModelCategoryUuids { get; set; } = null!;
    [Required] public IFormFile Model { get; set; } = null!;
}