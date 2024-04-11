namespace Basket.API.Extensions
{
    public class JwtOptions
    {
        public string Secret { get; set; } = string.Empty; // Secret key used for signing JWTs
        public int ExpiryMinutes { get; set; } = 180;// Expiration time for JWTs in minutes
    }
}
