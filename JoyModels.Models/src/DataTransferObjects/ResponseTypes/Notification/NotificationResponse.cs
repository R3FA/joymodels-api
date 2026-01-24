using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Notification;

public class NotificationResponse
{
    public Guid Uuid { get; set; }
    public UsersResponse Actor { get; set; } = null!;
    public UsersResponse TargetUser { get; set; } = null!;
    public string NotificationType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public Guid RelatedEntityUuid { get; set; }
    public string RelatedEntityType { get; set; } = string.Empty;
}