using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.ModelAvailability;

public class ModelAvailabilitySearchRequest : PaginationRequest
{
    [MaxLength(50, ErrorMessage = "AvailabilityName cannot exceed 50 characters.")]
    public string? AvailabilityName { get; set; } = string.Empty;
}