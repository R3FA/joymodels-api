using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Sso;

public class SsoSetRoleRequest
{
    [Required] public Guid UserUuid { get; set; }
    [Required] public Guid DesignatedUserRoleUuid { get; set; }
}