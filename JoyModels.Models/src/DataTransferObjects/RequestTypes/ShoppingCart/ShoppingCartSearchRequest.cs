using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.ShoppingCart;

public class ShoppingCartSearchRequest : PaginationRequest
{
    [MaxLength(100, ErrorMessage = "ModelName cannot exceed 100 characters.")]
    public string? ModelName { get; set; }
}