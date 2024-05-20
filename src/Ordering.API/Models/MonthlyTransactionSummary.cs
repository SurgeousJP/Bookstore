using System.Text.Json.Serialization;

namespace Ordering.API.Models
{
    public class MonthlyTransactionSummary
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public int MonthOfYearNumber { get; set; }
        public string Mont1hOfYear { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
