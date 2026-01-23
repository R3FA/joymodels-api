using JoyModels.Models.DataTransferObjects.Settings;
using Stripe;

namespace JoyModels.API.Setups;

public static class StripeSetup
{
    public static StripeSettingsDetails RegisterStripeDetails(IConfiguration configuration)
    {
        var stripeSection = configuration.GetSection("Stripe");

        var secretKey = stripeSection["SecretKey"];
        var webhookSecret = stripeSection["WebhookSecret"];
        var currency = stripeSection["Currency"];

        if (string.IsNullOrWhiteSpace(secretKey) || string.IsNullOrWhiteSpace(webhookSecret))
            throw new ApplicationException("Stripe settings are not configured!");

        StripeConfiguration.ApiKey = secretKey;

        return new StripeSettingsDetails
        {
            SecretKey = secretKey,
            WebhookSecret = webhookSecret,
            Currency = currency ?? "usd"
        };
    }
}