using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.Sso;

public class SsoGetByUuid
{
    [Required] public Guid UserUuid { get; set; }
}