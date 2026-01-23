using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Order;

public class OrderAdminSearchRequest : PaginationRequest
{
    public Guid? UserUuid { get; set; }

    [MaxLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
    public string? Status { get; set; }

    [MaxLength(255, ErrorMessage = "StripePaymentIntentId cannot exceed 255 characters.")]
    public string? StripePaymentIntentId { get; set; }
}