namespace JoyModels.Models.DataTransferObjects.RequestTypes.Report;

public class ReportSearchRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Status { get; set; }
    public string? ReportedEntityType { get; set; }
    public string? Reason { get; set; }
}