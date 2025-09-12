using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.Sso;

public class SsoVerify
{
    [Required] public string PendingUserUuid { get; set; } = null!;
    [Required] public string UserUuid { get; set; } = null!;
    [Required] public string OtpCode { get; set; } = null!;
}