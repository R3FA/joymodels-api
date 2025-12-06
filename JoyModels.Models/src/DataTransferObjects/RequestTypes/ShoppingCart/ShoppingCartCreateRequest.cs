using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.ShoppingCart;

public class ShoppingCartCreateRequest
{
    [Required] public Guid ModelUuid { get; set; }
}