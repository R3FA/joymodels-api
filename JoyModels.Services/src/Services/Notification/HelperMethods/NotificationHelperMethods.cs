using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.Notification;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using JoyModels.Services.Validation;
using Microsoft.EntityFrameworkCore;
using NotificationEntity = JoyModels.Models.Database.Entities.Notification;

namespace JoyModels.Services.Services.Notification.HelperMethods;

public static class NotificationHelperMethods
{
    public static async Task<NotificationEntity> GetNotificationEntity(
        JoyModelsDbContext context,
        UserAuthValidation userAuthValidation,
        Guid notificationUuid)
    {
        var userUuid = userAuthValidation.GetUserClaimUuid();

        var entity = await context.Notifications
            .AsNoTracking()
            .Include(x => x.Actor)
            .ThenInclude(x => x.UserRoleUu)
            .Include(x => x.TargetUser)
            .ThenInclude(x => x.UserRoleUu)
            .FirstOrDefaultAsync(x => x.Uuid == notificationUuid && x.TargetUserUuid == userUuid);

        return entity ??
               throw new KeyNotFoundException($"Notification with UUID `{notificationUuid}` does not exist.");
    }

    public static async Task<PaginationBase<NotificationEntity>> SearchNotificationEntities(
        JoyModelsDbContext context,
        UserAuthValidation userAuthValidation,
        NotificationSearchRequest request)
    {
        var userUuid = userAuthValidation.GetUserClaimUuid();

        var baseQuery = context.Notifications
            .AsNoTracking()
            .Include(x => x.Actor)
            .ThenInclude(x => x.UserRoleUu)
            .Include(x => x.TargetUser)
            .ThenInclude(x => x.UserRoleUu)
            .Where(x => x.TargetUserUuid == userUuid);

        if (request.IsRead.HasValue)
            baseQuery = baseQuery.Where(x => x.IsRead == request.IsRead.Value);

        if (!string.IsNullOrEmpty(request.NotificationType))
            baseQuery = baseQuery.Where(x => x.NotificationType == request.NotificationType);

        var orderedQuery = GlobalHelperMethods<NotificationEntity>.OrderBy(baseQuery, request.OrderBy);

        return await PaginationBase<NotificationEntity>.CreateAsync(
            orderedQuery,
            request.PageNumber,
            request.PageSize,
            request.OrderBy);
    }
}