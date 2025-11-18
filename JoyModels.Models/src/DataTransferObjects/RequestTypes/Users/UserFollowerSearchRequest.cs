using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Users;

public class UserFollowerSearchRequest : PaginationRequest
{
    [Required] public Guid TargetUserUuid { get; set; }
    public string? Nickname { get; set; }
}