using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ordering.API.Models.Order;

public partial class OrderStatus
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
