using JoyModels.Models.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.ModelAvailability;

public class ModelAvailabilitySearchRequest : PaginationBaseRequest
{
    public string? AvailabilityName { get; set; } = string.Empty;
}