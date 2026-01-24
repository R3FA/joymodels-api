using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.Notification;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Notification;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.Notification.HelperMethods;
using JoyModels.Services.Validation;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.Notification;

public class NotificationService(
    JoyModelsDbContext context,
    IMapper mapper,
    UserAuthValidation userAuthValidation)
    : INotificationService
{
    public async Task<NotificationResponse> GetByUuid(Guid notificationUuid)
    {
        var entity = await NotificationHelperMethods.GetNotificationEntity(
            context, userAuthValidation, notificationUuid);
        return mapper.Map<NotificationResponse>(entity);
    }

    public async Task<PaginationResponse<NotificationResponse>> Search(NotificationSearchRequest request)
    {
        var entities = await NotificationHelperMethods.SearchNotificationEntities(
            context, userAuthValidation, request);
        return mapper.Map<PaginationResponse<NotificationResponse>>(entities);
    }

    public async Task<int> GetUnreadCount()
    {
        var userUuid = userAuthValidation.GetUserClaimUuid();
        return await context.Notifications
            .AsNoTracking()
            .CountAsync(x => x.TargetUserUuid == userUuid && !x.IsRead);
    }

    public async Task MarkAsRead(Guid notificationUuid)
    {
        var userUuid = userAuthValidation.GetUserClaimUuid();
        var notification = await context.Notifications
            .FirstOrDefaultAsync(x => x.Uuid == notificationUuid && x.TargetUserUuid == userUuid);

        if (notification == null)
            throw new KeyNotFoundException($"Notification with UUID `{notificationUuid}` does not exist.");

        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
    }

    public async Task MarkAllAsRead()
    {
        var userUuid = userAuthValidation.GetUserClaimUuid();
        await context.Notifications
            .Where(x => x.TargetUserUuid == userUuid && !x.IsRead)
            .ExecuteUpdateAsync(x => x
                .SetProperty(n => n.IsRead, true)
                .SetProperty(n => n.ReadAt, DateTime.UtcNow));
    }

    public async Task Delete(Guid notificationUuid)
    {
        var userUuid = userAuthValidation.GetUserClaimUuid();
        var deleted = await context.Notifications
            .Where(x => x.Uuid == notificationUuid && x.TargetUserUuid == userUuid)
            .ExecuteDeleteAsync();

        if (deleted == 0)
            throw new KeyNotFoundException($"Notification with UUID `{notificationUuid}` does not exist.");
    }
}