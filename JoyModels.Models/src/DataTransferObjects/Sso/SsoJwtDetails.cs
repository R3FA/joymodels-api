using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.Sso;

public class SsoJwtDetails
{
    [Required] public string JwtSigningKey { get; set; } = string.Empty;
    [Required] public string JwtIssuer { get; set; } = string.Empty;
    [Required] public string JwtAudience { get; set; } = string.Empty;
}