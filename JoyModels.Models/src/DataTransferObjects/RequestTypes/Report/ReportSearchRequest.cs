using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Report;

public class ReportSearchRequest : PaginationRequest
{
    public string? Status { get; set; }
    public string? ReportedEntityType { get; set; }
    public string? Reason { get; set; }
}