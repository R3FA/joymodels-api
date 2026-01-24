using JoyModels.Models.DataTransferObjects.RequestTypes.Notification;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Notification;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;

namespace JoyModels.Services.Services.Notification;

public interface INotificationService
{
    Task<NotificationResponse> GetByUuid(Guid notificationUuid);
    Task<PaginationResponse<NotificationResponse>> Search(NotificationSearchRequest request);
    Task<int> GetUnreadCount();
    Task MarkAsRead(Guid notificationUuid);
    Task MarkAllAsRead();
    Task Delete(Guid notificationUuid);
}