using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Sso;

public class SsoSearchRequest : PaginationRequest
{
    [MaxLength(50, ErrorMessage = "Nickname cannot exceed 50 characters.")]
    public string? Nickname { get; set; }

    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
    public string? Email { get; set; }
}