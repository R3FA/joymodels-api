using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Categories;

public class CategoryPatchRequest
{
    [Required] public Guid Uuid { get; set; }

    [Required, MaxLength(50, ErrorMessage = "CategoryName cannot exceed 50 characters.")]
    public string CategoryName { get; set; } = string.Empty;
}