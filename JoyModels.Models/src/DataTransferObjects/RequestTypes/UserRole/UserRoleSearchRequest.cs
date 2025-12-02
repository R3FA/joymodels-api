using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.UserRole;

public class UserRoleSearchRequest : PaginationRequest
{
    [MaxLength(50, ErrorMessage = "RoleName cannot exceed 50 characters.")]
    public string? RoleName { get; set; }
}