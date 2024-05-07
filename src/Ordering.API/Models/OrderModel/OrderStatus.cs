using System.Text.Json.Serialization;

namespace Ordering.API.Models.OrderModel;

public partial class OrderStatus
{
    public static int ORDER_SUBMITTED = 1;
    public static int ORDER_STOCK_CONFIRMED = 2;
    public static int ORDER_PAID = 3;
    public static int ORDER_SHIPPED = 4;
    public static int ORDER_STATUS_CANCELLED = 5;

    public long Id { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
