using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.ResponseTypes.UserRole;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Sso;

public class SsoUserResponse
{
    [Required] public Guid Uuid { get; set; }
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string? LastName { get; set; }
    [Required] public string NickName { get; set; } = null!;
    [Required] public string Email { get; set; } = null!;
    [Required] public DateTime CreatedAt { get; set; }
    [Required] public string UserAccessToken { get; set; } = null!;
    [Required] public Guid UserRoleUuid { get; set; }
    [Required] public virtual UserRoleResponse UserRole { get; set; } = null!;
}