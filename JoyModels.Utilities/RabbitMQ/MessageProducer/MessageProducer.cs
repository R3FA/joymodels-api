using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace JoyModels.Utilities.RabbitMQ.MessageProducer;

public class MessageProducer : IMessageProducer
{
    public async Task SendMessage<T>(string queue, T message)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var jsonString = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonString);

        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queue, body: body);
    }
}