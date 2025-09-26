using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Sso;

public abstract class SsoBasePaginationRequest
{
    [Required, DefaultValue(1)] public int PageNumber { get; set; }
    [Required, DefaultValue(5)] public int PageSize { get; set; }
}