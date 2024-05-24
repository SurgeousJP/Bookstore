namespace Ordering.API.Models.ReportModel
{
    public class TopProduct
    {
        public int BookId { get; set; }
        public string? Title { get; set; }
        public int TotalQuantityBought { get; set; }
    }
}
