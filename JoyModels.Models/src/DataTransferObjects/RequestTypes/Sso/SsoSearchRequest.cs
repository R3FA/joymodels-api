using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Sso;

public class SsoSearchRequest : PaginationRequest
{
    public string? Nickname { get; set; }
    public string? Email { get; set; }
}