using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.Sso;

public class SsoRequestPasswordChange
{
    [Required] public Guid UserUuid { get; set; }
    [Required] public string NewPassword { get; set; } = null!;
    [Required] public string ConfirmNewPassword { get; set; } = null!;
}