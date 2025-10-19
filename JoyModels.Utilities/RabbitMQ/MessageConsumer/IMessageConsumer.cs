namespace JoyModels.Utilities.RabbitMQ.MessageConsumer;

public interface IMessageConsumer
{
    public Task ReceiveMessage(string queue);
}