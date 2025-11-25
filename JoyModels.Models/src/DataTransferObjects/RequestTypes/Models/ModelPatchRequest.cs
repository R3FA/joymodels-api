using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Models;

public class ModelPatchRequest
{
    [Required] public Guid Uuid { get; set; }

    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string? Name { get; set; }

    [MaxLength(1500, ErrorMessage = "Description cannot exceed 1500 characters.")]
    public string? Description { get; set; }

    public decimal? Price { get; set; }
    public Guid? ModelAvailabilityUuid { get; set; }
    [MaxLength(5)] public List<Guid>? ModelCategoriesToDelete { get; set; }
    [MaxLength(5)] public List<Guid>? ModelCategoriesToInsert { get; set; }
    [MaxLength(8)] public List<string>? ModelPictureLocationsToDelete { get; set; }
    [MaxLength(8)] public List<IFormFile>? ModelPictureToInsert { get; set; }
}