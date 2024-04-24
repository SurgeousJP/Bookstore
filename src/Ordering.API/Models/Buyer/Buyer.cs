using System;
using System.Collections.Generic;
using Ordering.API.Models.Order;

namespace Ordering.API;

public partial class Buyer
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
}
