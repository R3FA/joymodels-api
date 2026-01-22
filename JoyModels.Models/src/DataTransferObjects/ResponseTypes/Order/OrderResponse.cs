using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Order;

public class OrderResponse
{
    public Guid Uuid { get; set; }
    public ModelResponse Model { get; set; } = null!;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string StripePaymentIntentId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}