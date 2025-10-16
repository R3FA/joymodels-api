using JoyModels.Models.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Users;

public class UsersSearchRequest : PaginationBaseRequest
{
    public string? Nickname { get; set; }
}