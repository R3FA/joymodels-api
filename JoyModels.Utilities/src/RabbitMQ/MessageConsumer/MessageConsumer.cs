using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace JoyModels.Utilities.RabbitMQ.MessageConsumer;

public class MessageConsumer(ILogger<MessageConsumer> logger) : IMessageConsumer
{
    public async Task ReceiveMessage(string queue)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            await ((AsyncEventingBasicConsumer)sender)
                .Channel
                .BasicAckAsync(eventArgs.DeliveryTag, multiple: false);

            logger.LogDebug($"Received message from queue {queue} : {message}");
        };

        await channel.BasicConsumeAsync(queue: queue, autoAck: false, consumer: consumer);
    }
}