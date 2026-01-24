namespace JoyModels.Models.DataTransferObjects.RequestTypes.Report;

public class ReportPatchStatusRequest
{
    public Guid ReportUuid { get; set; }
    public string Status { get; set; } = string.Empty;
}