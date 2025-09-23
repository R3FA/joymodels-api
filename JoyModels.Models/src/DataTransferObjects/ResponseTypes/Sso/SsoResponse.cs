using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Sso;

public class SsoResponse
{
    [Required] public Guid Uuid { get; set; }
    [Required] public Guid UserUuid { get; set; }
    [Required] public virtual SsoUserResponse User { get; set; } = null!;
}