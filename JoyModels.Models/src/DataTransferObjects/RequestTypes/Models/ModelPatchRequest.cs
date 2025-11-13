using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Models;

public class ModelPatchRequest
{
    [Required] public Guid Uuid { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public Guid? ModelAvailabilityUuid { get; set; }
    [MaxLength(5)] public List<Guid>? ModelCategoriesToDelete { get; set; }
    [MaxLength(5)] public List<Guid>? ModelCategoriesToInsert { get; set; }
    [MaxLength(8)] public List<string>? ModelPictureLocationsToDelete { get; set; }
    [MaxLength(8)] public List<IFormFile>? ModelPictureToInsert { get; set; }
}