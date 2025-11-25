using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Categories;

public class CategoryCreateRequest
{
    [Required, MaxLength(50, ErrorMessage = "CategoryName cannot exceed 50 characters.")]
    public string CategoryName { get; set; } = string.Empty;
}