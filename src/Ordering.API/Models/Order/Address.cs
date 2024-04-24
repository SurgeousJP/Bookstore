using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ordering.API.Models.Order;

public partial class Address
{
    public long Id { get; set; }

    public string Street { get; set; } = null!;

    public string? Ward { get; set; }

    public string? District { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public string? ZipCode { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
