using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Sso;

public class SsoLoginRequest
{
    [Required, MaxLength(50, ErrorMessage = "Nickname cannot exceed 50 characters.")]
    public string Nickname { get; set; } = string.Empty;

    [Required, MaxLength(50, ErrorMessage = "Password cannot exceed 50 characters.")]
    public string Password { get; set; } = string.Empty;
}