using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Sso;

public class SsoVerifyRequest
{
    [Required] public Guid UserUuid { get; set; }

    [Required, MaxLength(12, ErrorMessage = "OTP Code cannot exceed 12 characters.")]
    public string OtpCode { get; set; } = string.Empty;

    [Required] public string UserRefreshToken { get; set; } = string.Empty;
}