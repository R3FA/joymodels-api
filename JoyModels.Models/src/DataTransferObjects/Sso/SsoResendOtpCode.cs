using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.Sso;

public class SsoResendOtpCode
{
    [Required] public Guid UserUuid { get; set; }
}