namespace JoyModels.Utilities.RabbitMQ.MessageProducer;

public interface IMessageProducer
{
    public Task SendMessage<T>(string queue, T message);
}