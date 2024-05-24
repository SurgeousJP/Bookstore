namespace Ordering.API.Models.DTOs
{
    public class PaymentMethodDTO
    {
        public long Id { get; set; }
        public string Alias { get; set; } = null!;

        public string? CardNumber { get; set; }

        public string? SecurityNumber { get; set; }

        public string? CardHoldername { get; set; }

        public DateOnly? Expiration { get; set; }

        public long? CardTypeId { get; set; }

        public string? CardTypeName { get; set; }
    }
}
