using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Sso;

public class SsoSearchRequest
{
    public string? Nickname { get; set; }
    public string? Email { get; set; }
    [Required, DefaultValue(1)] public int PageNumber { get; set; }
    [Required, DefaultValue(5)] public int PageSize { get; set; }
}