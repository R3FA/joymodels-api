using JoyModels.Models.DataTransferObjects.RabbitMq;

namespace JoyModels.API.Setups;

public static class RabbitMqSetup
{
    public static RabbitMqDetails RegisterRabbitMqDetails(IConfiguration configuration)
    {
        var rabbitMqDetails = configuration.GetSection("Connection:RabbitMQ");
        var rabbitMqHost = rabbitMqDetails["Host"];
        var rabbitMqUser = rabbitMqDetails["User"];
        var rabbitMqVirtualHost = rabbitMqDetails["VirtualHost"];
        var rabbitMqPassword = rabbitMqDetails["Password"];

        if (string.IsNullOrWhiteSpace(rabbitMqHost)
            || string.IsNullOrWhiteSpace(rabbitMqUser)
            || string.IsNullOrWhiteSpace(rabbitMqVirtualHost)
            || string.IsNullOrWhiteSpace(rabbitMqPassword))
            throw new ApplicationException("RabbitMQ credentials are not configured!");

        return new RabbitMqDetails()
        {
            Host = rabbitMqHost,
            User = rabbitMqUser,
            VirtualHost = rabbitMqVirtualHost,
            Password = rabbitMqPassword,
        };
    }
}