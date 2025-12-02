using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.UserRole;

public class UserRolePatchRequest
{
    [Required] public Guid RoleUuid { get; set; }

    [Required, MaxLength(50, ErrorMessage = "RoleName cannot exceed 50 characters.")]
    public string RoleName { get; set; } = string.Empty;
}