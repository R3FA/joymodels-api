namespace JoyModels.Utilities.RabbitMQ.MessageConsumer;

/// <summary>
/// The callback that can be ran after a message is received on a RabbitMQ queue.
/// </summary>
public delegate Task MessageReceivedCallback(string message);

public interface IMessageConsumer
{
    public Task ReceiveMessage(string queue, MessageReceivedCallback callback);
}