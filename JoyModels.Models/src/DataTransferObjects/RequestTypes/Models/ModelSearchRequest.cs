using JoyModels.Models.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Models;

public class ModelSearchRequest : PaginationBaseRequest
{
    public string? ModelName { get; set; } = string.Empty;
}