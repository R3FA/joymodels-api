namespace JoyModels.Models.DataTransferObjects.RequestTypes.Notification;

public class CreateNotificationRequest
{
    public Guid ActorUuid { get; set; }
    public Guid TargetUserUuid { get; set; }
    public string NotificationType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Guid RelatedEntityUuid { get; set; }
    public string RelatedEntityType { get; set; } = string.Empty;
}