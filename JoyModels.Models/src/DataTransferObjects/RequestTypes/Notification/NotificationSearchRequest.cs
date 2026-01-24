using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Notification;

public class NotificationSearchRequest : PaginationRequest
{
    public bool? IsRead { get; set; }
    public string? NotificationType { get; set; }
}