using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Users;

public class UsersSearchRequest : PaginationRequest
{
    [MaxLength(50, ErrorMessage = "Nickname cannot exceed 50 characters.")]
    public string? Nickname { get; set; }
}