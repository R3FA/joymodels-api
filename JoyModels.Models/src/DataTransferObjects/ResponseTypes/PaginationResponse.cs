using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes;

public class PaginationResponse<T>
{
    [Required] public int PageNumber { get; set; }
    [Required] public int PageSize { get; set; }
    [Required] public int TotalRecords { get; set; }
    [Required] public int TotalPages { get; set; }
    [Required] public bool HasPreviousPage { get; set; }
    [Required] public bool HasNextPage { get; set; }
    [Required] public List<T> Data { get; set; } = null!;
}