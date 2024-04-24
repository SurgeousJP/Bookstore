namespace Ordering.API.Models.DTOs
{
    public class CreateOrderDTO
    {
        public long AddressId { get; set; }

        public Guid? BuyerId { get; set; }

        public long? OrderStatusId { get; set; }

        public string? Description { get; set; }

        public long? PaymentMethodId { get; set; }

        public DateOnly? OrderDate { get; set; }

        public float? TotalAmount { get; set; }

        public long? ShippingId { get; set; }
    }
}
