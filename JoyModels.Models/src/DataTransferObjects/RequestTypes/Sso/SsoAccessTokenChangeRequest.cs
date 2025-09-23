using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Sso;

public class SsoAccessTokenChangeRequest
{
    [Required] public Guid UserUuid { get; set; }
    [Required] public string UserRefreshToken { get; set; } = string.Empty;
}