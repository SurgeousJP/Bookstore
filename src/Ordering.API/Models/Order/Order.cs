using System;
using System.Collections.Generic;

namespace Ordering.API.Models.Order;

public partial class Order
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

    public virtual Address Address { get; set; } = null!;

    public virtual Buyer? Buyer { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual OrderStatus? OrderStatus { get; set; }

    public virtual PaymentMethod? PaymentMethod { get; set; }
}
