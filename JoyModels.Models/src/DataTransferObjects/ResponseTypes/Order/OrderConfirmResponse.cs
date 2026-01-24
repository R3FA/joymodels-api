namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Order;

public class OrderConfirmResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? OrderUuid { get; set; }
}