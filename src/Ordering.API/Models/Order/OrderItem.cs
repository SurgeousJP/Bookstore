using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ordering.API.Models.Order;

public partial class OrderItem
{
    public long Id { get; set; }

    public long BookId { get; set; }

    public string? Title { get; set; }

    public float? UnitPrice { get; set; }

    public float? OldUnitPrice { get; set; }

    public float? TotalUnitPrice { get; set; }

    public int? Quantity { get; set; }

    public string? ImageUrl { get; set; }

    public long? OrderId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual Order? Order { get; set; }
}
