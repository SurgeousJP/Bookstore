using Ordering.API.Models.OrderModel;

namespace Ordering.API.Models.DTOs
{
    public class OrderDetailDTO
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

        public string Street { get; set; } = null!;

        public string? Ward { get; set; }

        public string? District { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? ZipCode { get; set; }

        public string BuyerName { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public string OrderStatusName { get; set; }

        public string PaymentMethodName { get; set; }
    }
}
