namespace BookCatalog.API.Model
{
    public class JwtOptions
    {
        public string Secret { get; set; } = string.Empty; // Secret key used for signing JWTs
        public int ExpiryMinutes { get; set; } = 60;// Expiration time for JWTs in minutes
    }
}
