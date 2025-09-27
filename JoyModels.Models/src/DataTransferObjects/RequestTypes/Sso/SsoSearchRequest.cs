using JoyModels.Models.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Sso;

public class SsoSearchRequest : PaginationBaseRequest
{
    public string? Nickname { get; set; }
    public string? Email { get; set; }
}