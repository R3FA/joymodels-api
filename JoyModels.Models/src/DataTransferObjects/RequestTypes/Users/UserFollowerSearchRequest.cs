using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Users;

public class UserFollowerSearchRequest : PaginationRequest
{
    [Required] public Guid TargetUserUuid { get; set; }

    [MaxLength(50, ErrorMessage = "Nickname cannot exceed 50 characters.")]
    public string? Nickname { get; set; }
}