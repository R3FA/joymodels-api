using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.Sso;

public class SsoLogoutRequest
{
    [Required] public Guid UserUuid { get; set; }
    [Required] public string UserRefreshToken { get; set; } = string.Empty;
}