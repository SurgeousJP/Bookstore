using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ordering.API;

public partial class Cardtype
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
}
