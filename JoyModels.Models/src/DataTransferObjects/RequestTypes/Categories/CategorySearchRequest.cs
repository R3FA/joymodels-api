using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Categories;

public class CategorySearchRequest : PaginationRequest
{
    public string? CategoryName { get; set; }
}