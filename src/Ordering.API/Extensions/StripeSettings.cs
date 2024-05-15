namespace Ordering.API.Extensions
{
    public class StripeSettings
    {
        public string StripeAPIKey { get; set; } = string.Empty;
        public string WebhookSecretKey { get; set; } = string.Empty;
    }
}
