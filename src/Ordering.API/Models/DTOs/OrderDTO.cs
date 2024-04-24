using Ordering.API.Models.Order;

namespace Ordering.API.Models.DTOs
{
    public class OrderDTO
    {
        public long Id { get; set; }

        public long AddressId { get; set; }

        public Guid? BuyerId { get; set; }

        public long? OrderStatusId { get; set; }

        public string? Description { get; set; }

        public long? PaymentMethodId { get; set; }

        public DateOnly? OrderDate { get; set; }

        public float? TotalAmount { get; set; }

        public long? ShippingId { get; set; }

        public string? BuyerName { get; set; }

        public string? OrderStatusName { get; set; }
    }
}
