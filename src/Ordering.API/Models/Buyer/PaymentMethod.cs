using System;
using System.Collections.Generic;
using Ordering.API.Models.Order;

namespace Ordering.API;

public partial class PaymentMethod
{
    public long Id { get; set; }

    public string Alias { get; set; } = null!;

    public string? CardNumber { get; set; }

    public string? SecurityNumber { get; set; }

    public string? CardHoldername { get; set; }

    public DateOnly? Expiration { get; set; }

    public long? CardTypeId { get; set; }

    public Guid? BuyerId { get; set; }

    public virtual Buyer? Buyer { get; set; }

    public virtual Cardtype? CardType { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
