using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.UserRole;

public class UserRoleGet
{
    [Required] public Guid Uuid { get; set; }
    [Required] public string RoleName { get; set; } = null!;
}