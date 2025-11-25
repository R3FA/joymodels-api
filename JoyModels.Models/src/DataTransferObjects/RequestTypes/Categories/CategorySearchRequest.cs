using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Categories;

public class CategorySearchRequest : PaginationRequest
{
    [MaxLength(50, ErrorMessage = "CategoryName cannot exceed 50 characters.")]
    public string? CategoryName { get; set; }
}