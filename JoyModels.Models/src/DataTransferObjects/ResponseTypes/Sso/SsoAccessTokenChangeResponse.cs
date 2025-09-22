using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Sso;

public class SsoAccessTokenChangeResponse
{
    [Required] public string UserAccessToken { get; set; } = string.Empty;
}