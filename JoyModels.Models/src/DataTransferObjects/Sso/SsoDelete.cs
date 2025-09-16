using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.Sso;

public class SsoDelete
{
    [Required] public Guid UserUuid { get; set; }
}