using System.ComponentModel.DataAnnotations;
using JoyModels.Models.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Categories;

public class CategorySearchRequest : PaginationBaseRequest
{
    public string? CategoryName { get; set; }
}