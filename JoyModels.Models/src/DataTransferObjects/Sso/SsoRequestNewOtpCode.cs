using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.Sso;

public class SsoRequestNewOtpCode
{
    [Required] public Guid UserUuid { get; set; }
}