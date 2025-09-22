using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Sso;

public class SsoVerifyRequest
{
    [Required] public Guid UserUuid { get; set; }
    [Required] public string OtpCode { get; set; } = null!;
}