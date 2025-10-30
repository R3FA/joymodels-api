using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Categories;

public class CategoryPatchRequest
{
    [Required] public Guid Uuid { get; set; }
    [Required] public string CategoryName { get; set; } = string.Empty;
}