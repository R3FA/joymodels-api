using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Order;

public class OrderSearchRequest : PaginationRequest
{
    [MaxLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
    public string? Status { get; set; }
}