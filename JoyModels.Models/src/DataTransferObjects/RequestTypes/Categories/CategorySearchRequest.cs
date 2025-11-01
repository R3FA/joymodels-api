using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Categories;

public class CategorySearchRequest : PaginationRequest
{
    [MaxLength(100)] public string? CategoryName { get; set; }
}