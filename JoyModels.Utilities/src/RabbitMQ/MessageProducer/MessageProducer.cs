using System.Text;
using System.Text.Json;
using JoyModels.Models.DataTransferObjects.RabbitMq;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace JoyModels.Utilities.RabbitMQ.MessageProducer;

public class MessageProducer(ILogger<MessageProducer> logger, RabbitMqDetails rabbitMqDetails) : IMessageProducer
{
    public async Task SendMessage<T>(string queue, T message)
    {
        var factory = new ConnectionFactory
        {
            HostName = rabbitMqDetails.Host,
            UserName = rabbitMqDetails.User,
            VirtualHost = rabbitMqDetails.VirtualHost,
            Password = rabbitMqDetails.Password,
        };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

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