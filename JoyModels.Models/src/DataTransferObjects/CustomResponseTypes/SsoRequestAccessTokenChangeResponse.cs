using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.CustomResponseTypes;

public class SsoRequestAccessTokenChangeResponse
{
    [Required] public string UserAccessToken { get; set; } = string.Empty;
}