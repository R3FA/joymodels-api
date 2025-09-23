using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Sso;

public class SsoPasswordChangeRequest
{
    [Required] public Guid UserUuid { get; set; }
    [Required] public string NewPassword { get; set; } = null!;
    [Required] public string ConfirmNewPassword { get; set; } = null!;
}