namespace JoyModels.Models.Database.Entities;

public partial class Notification
{
    public Guid Uuid { get; set; }
    public Guid ActorUuid { get; set; }
    public Guid TargetUserUuid { get; set; }
    public string NotificationType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }

    public Guid RelatedEntityUuid { get; set; }
    public string RelatedEntityType { get; set; } = string.Empty;

    public virtual User Actor { get; set; } = null!;
    public virtual User TargetUser { get; set; } = null!;
}