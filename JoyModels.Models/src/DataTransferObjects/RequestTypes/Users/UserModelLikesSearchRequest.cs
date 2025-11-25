using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Users;

public class UserModelLikesSearchRequest : PaginationRequest
{
    [Required] public Guid UserUuid { get; set; }

    [MaxLength(100, ErrorMessage = "ModelName cannot exceed 100 characters.")]
    public string? ModelName { get; set; }
}