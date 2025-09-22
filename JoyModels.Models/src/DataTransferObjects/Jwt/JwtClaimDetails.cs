using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.Jwt;

public class JwtClaimDetails
{
    [Required] public string JwtSigningKey { get; set; } = string.Empty;
    [Required] public string JwtIssuer { get; set; } = string.Empty;
    [Required] public string JwtAudience { get; set; } = string.Empty;
}