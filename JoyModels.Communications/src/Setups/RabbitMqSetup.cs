using JoyModels.Models.DataTransferObjects.RabbitMq;

namespace JoyModels.Communications.Setups;

public static class RabbitMqSetup
{
    public static RabbitMqDetails RegisterRabbitMqDetails(IConfiguration configuration)
    {
        var rabbitMqDetails = configuration.GetSection("Connection:RabbitMQ");
        var rabbitMqHost = rabbitMqDetails["Host"];
        var rabbitMqUser = rabbitMqDetails["User"];
        var rabbitMqVirtualHost = rabbitMqDetails["VirtualHost"];
        var rabbitMqPassword = rabbitMqDetails["Password"];

        if (string.IsNullOrEmpty(rabbitMqHost)
            || string.IsNullOrEmpty(rabbitMqUser)
            || string.IsNullOrEmpty(rabbitMqVirtualHost)
            || string.IsNullOrEmpty(rabbitMqPassword))
            throw new ApplicationException("RabbitMQ details are not configured!");

        return new RabbitMqDetails()
        {
            Host = rabbitMqHost,
            User = rabbitMqUser,
            VirtualHost = rabbitMqVirtualHost,
            Password = rabbitMqPassword,
        };
    }
}