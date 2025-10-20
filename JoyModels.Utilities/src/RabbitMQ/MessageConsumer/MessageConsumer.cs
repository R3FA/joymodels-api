using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace JoyModels.Utilities.RabbitMQ.MessageConsumer;

public class MessageConsumer : IMessageConsumer
{
    public async Task ReceiveMessage(string queue)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            _ = Encoding.UTF8.GetString(body);
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(queue, autoAck: true, consumer: consumer);
    }
}