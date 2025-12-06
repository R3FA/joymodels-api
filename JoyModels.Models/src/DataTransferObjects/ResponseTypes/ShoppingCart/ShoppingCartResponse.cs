using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.ShoppingCart;

public class ShoppingCartResponse
{
    public Guid Uuid { get; set; }
    public ModelResponse Model { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}