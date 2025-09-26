namespace JoyModels.Models.DataTransferObjects.RequestTypes.Sso;

public class SsoSearchRequest : SsoBasePaginationRequest
{
    public string? Nickname { get; set; }
    public string? Email { get; set; }
}