using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Categories;

public class CategoryCreateRequest
{
    [MaxLength(100)] [Required] public string CategoryName { get; set; } = string.Empty;
}