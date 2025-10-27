namespace JoyModels.Models.DataTransferObjects.RabbitMq;

public class RabbitMqDetails
{
    public string Host { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}