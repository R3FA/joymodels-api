using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Sso;

public class SsoLoginRequest
{
    [Required] public string Nickname { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}