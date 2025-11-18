using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Users;

public class UsersFollowRequest
{
    [Required] public Guid OriginUserUuid { get; set; }
    [Required] public Guid TargetUserUuid { get; set; }
}