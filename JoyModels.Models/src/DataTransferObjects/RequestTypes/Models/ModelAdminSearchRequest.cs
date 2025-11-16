using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Models;

public class ModelAdminSearchRequest : PaginationRequest
{
    public string? ModelName { get; set; } = string.Empty;
}