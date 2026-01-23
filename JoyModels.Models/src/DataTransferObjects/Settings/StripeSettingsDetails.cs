namespace JoyModels.Models.DataTransferObjects.Settings;

public class StripeSettingsDetails
{
    public string SecretKey { get; set; } = string.Empty;
    public string WebhookSecret { get; set; } = string.Empty;
    public string Currency { get; set; } = "usd";
}