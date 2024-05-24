using System.Text.Json.Serialization;

namespace Ordering.API.Models.ReportModel
{
    public class MonthlyTransactionSummary
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public int MonthOfYearNumber { get; set; }
        public string MonthOfYear { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
