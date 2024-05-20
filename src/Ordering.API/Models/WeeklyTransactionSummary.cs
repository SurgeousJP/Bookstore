using System.Text.Json.Serialization;

namespace Ordering.API.Models
{
    public class WeeklyTransactionSummary
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public string DayOfWeekNumber { get; set; }
        public string DayOfWeek { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
