using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Sso;

public class SsoNewOtpCodeRequest
{
    [Required] public Guid UserUuid { get; set; }
}