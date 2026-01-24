namespace JoyModels.Models.Database.Entities;

public partial class Report
{
    public Guid Uuid { get; set; }
    public Guid ReporterUuid { get; set; }
    public string ReportedEntityType { get; set; } = string.Empty;
    public Guid ReportedEntityUuid { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public Guid? ReviewedByUuid { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual User Reporter { get; set; } = null!;
    public virtual User? ReviewedBy { get; set; }
}