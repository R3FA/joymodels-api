using System.Text.Json;
using JoyModels.Models.DataTransferObjects.RequestTypes.Email;
using JoyModels.Utilities.RabbitMQ.MessageConsumer;

namespace JoyModels.Communications.Services.BackgroundServices;

public class EmailBackgroundService(
    ILogger<EmailBackgroundService> logger,
    IEmailService emailService,
    IMessageConsumer messageConsumer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogDebug("Starting background service for email operations...");
        await messageConsumer.ReceiveMessage("send_email", HandleSendingEmail);

        while (!stoppingToken.IsCancellationRequested)
            await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task HandleSendingEmail(string message)
    {
        EmailSendRequest? emailMessage;

        try
        {
            emailMessage = JsonSerializer.Deserialize<EmailSendRequest>(message);
        }
        catch (Exception e)
        {
            logger.LogError($"Could not deserialize RabbitMQ message as an email message: {e}");
            return;
        }

        if (emailMessage == null)
        {
            logger.LogError("Could not deserialize RabbitMQ message as an empty email message");
            return;
        }

        await emailService.SendEmailAsync(emailMessage);
        logger.LogDebug($"Sent email to {emailMessage.To}");
    }
}