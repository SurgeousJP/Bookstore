namespace Ordering.API.Models.DTOs
{
    public class TransactionDetailDTO
    {
        public long Id { get; set; }
        public Guid BuyerId { get; set; }
        public decimal TotalAmount { get; set; }
        public long PaymentMethodId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? BuyerName { get; set; }
    }
}
