using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.UserRole;

public class UserRoleResponse
{
    [Required] public Guid Uuid { get; set; }
    [Required] public string RoleName { get; set; } = null!;
}