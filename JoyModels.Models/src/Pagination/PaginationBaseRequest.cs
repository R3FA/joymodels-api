using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.Pagination;

public abstract class PaginationBaseRequest
{
    [Required, DefaultValue(1)] public int PageNumber { get; set; }
    [Required, DefaultValue(5)] public int PageSize { get; set; }
    public string? OrderBy { get; set; }
}