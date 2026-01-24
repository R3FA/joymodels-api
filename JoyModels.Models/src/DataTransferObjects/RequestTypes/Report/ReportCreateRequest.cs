namespace JoyModels.Models.DataTransferObjects.RequestTypes.Report;

public class ReportCreateRequest
{
    public string ReportedEntityType { get; set; } = string.Empty;
    public Guid ReportedEntityUuid { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Description { get; set; }
}