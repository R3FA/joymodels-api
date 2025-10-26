using RabbitMQ.Client;

namespace JoyModels.Utilities.RabbitMQ;

public abstract class RabbitMqService()
{
    private static ConnectionFactory _connectionFactory = null!;

    public static void Init(string? hostname, string? username, string? password)
    {
        if (hostname == null || username == null || password == null)
            throw new Exception("Not all RabbitMQ connection details have been set!");

        _connectionFactory = new ConnectionFactory
        {
            HostName = hostname,
            UserName = username,
            Password = password,
            VirtualHost = username,
            Port = AmqpTcpEndpoint.UseDefaultPort,
        };
    }

    public static async Task<IConnection> CreateConnectionAsync()
    {
        return await _connectionFactory.CreateConnectionAsync();
    }
}