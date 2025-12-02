using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.UserRole;

public class UserRoleCreateRequest
{
    [Required, MaxLength(50, ErrorMessage = "RoleName cannot exceed 50 characters.")]
    public string RoleName { get; set; } = string.Empty;
}