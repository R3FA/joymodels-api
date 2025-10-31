using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.ModelAvailability;

public class ModelAvailabilitySearchRequest : PaginationRequest
{
    public string? AvailabilityName { get; set; } = string.Empty;
}