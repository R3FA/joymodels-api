using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Users;

public class UserModelLikesSearchRequest : PaginationRequest
{
    [Required] public Guid UserUuid { get; set; }
    public string? ModelName { get; set; }
}