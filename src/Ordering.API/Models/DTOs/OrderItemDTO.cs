namespace Ordering.API.Models.DTOs
{
    public class OrderItemDTO
    {
        public long BookId { get; set; }

        public string? Title { get; set; }

        public float? UnitPrice { get; set; }

        public float? OldUnitPrice { get; set; }

        public float? TotalUnitPrice { get; set; }

        public int? Quantity { get; set; }

        public string? ImageUrl { get; set; }
    }
}
