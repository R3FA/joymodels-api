namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Order;

public class OrderCheckoutResponse
{
    public string ClientSecret { get; set; } = string.Empty;
    public string EphemeralKey { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string PaymentIntentId { get; set; } = string.Empty;
}