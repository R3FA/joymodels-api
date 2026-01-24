using System.Text.Json;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.Notification;
using JoyModels.Utilities.RabbitMQ.MessageConsumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JoyModels.Communications.Services.BackgroundServices;

public class NotificationBackgroundService(
    ILogger<NotificationBackgroundService> logger,
    IMessageConsumer messageConsumer,
    IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogDebug("Starting background service for notification operations...");
        await messageConsumer.ReceiveMessage("create_notification", HandleCreateNotification);
    }

    private async Task HandleCreateNotification(string message)
    {
        CreateNotificationRequest? request;
        try
        {
            request = JsonSerializer.Deserialize<CreateNotificationRequest>(message);
        }
        catch (Exception e)
        {
            logger.LogError($"Could not deserialize RabbitMQ message as a notification message: {e}");
            return;
        }

        if (request == null)
        {
            logger.LogError("Could not deserialize RabbitMQ message as an empty notification message");
            return;
        }

        using var scope = serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<JoyModelsDbContext>();

        var notification = new Notification
        {
            Uuid = Guid.NewGuid(),
            ActorUuid = request.ActorUuid,
            TargetUserUuid = request.TargetUserUuid,
            NotificationType = request.NotificationType,
            Title = request.Title,
            Message = request.Message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow,
            RelatedEntityUuid = request.RelatedEntityUuid,
            RelatedEntityType = request.RelatedEntityType
        };

        await context.Notifications.AddAsync(notification);
        await context.SaveChangesAsync();

        logger.LogDebug($"Created notification for user {request.TargetUserUuid}: {request.Title}");
    }
}