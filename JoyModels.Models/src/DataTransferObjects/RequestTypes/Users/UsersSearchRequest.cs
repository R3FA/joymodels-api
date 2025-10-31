using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Users;

public class UsersSearchRequest : PaginationRequest
{
    public string? Nickname { get; set; }
}