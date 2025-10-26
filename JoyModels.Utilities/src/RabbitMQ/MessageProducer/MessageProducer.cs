using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace JoyModels.Utilities.RabbitMQ.MessageProducer;

public class MessageProducer(ILogger<MessageProducer> logger) : IMessageProducer
{
    public async Task SendMessage<T>(string queue, T message)
    {
        var factory = await RabbitMqService.CreateConnectionAsync();
        using var channel = await factory.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue,
            durable: true,
            exclusive: false,
            autoDelete: false, arguments: null);

        var jsonString = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonString);

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queue,
            mandatory: true,
            basicProperties: new BasicProperties { Persistent = true },
            body: body);

        logger.LogDebug($"Sent message on queue {queue} : {jsonString}");
    }
}