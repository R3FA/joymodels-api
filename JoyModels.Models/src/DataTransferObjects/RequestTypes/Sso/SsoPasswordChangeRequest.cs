using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Sso;

public class SsoPasswordChangeRequest
{
    [Required] public Guid UserUuid { get; set; }

    [Required, MaxLength(50, ErrorMessage = "NewPassword cannot exceed 50 characters.")]
    public string NewPassword { get; set; } = string.Empty;

    [Required, MaxLength(50, ErrorMessage = "ConfirmNewPassword cannot exceed 50 characters.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}