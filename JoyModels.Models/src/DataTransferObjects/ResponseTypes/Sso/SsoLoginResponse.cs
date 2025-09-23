using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Sso;

public class SsoLoginResponse
{
    [Required] public string AccessToken { get; set; } = null!;
    [Required] public string RefreshToken { get; set; } = null!;
}