using BookCatalog.API.Model;
using System.Text.Json.Serialization;

namespace BookCatalog.API.Queries.DTOs
{
    public partial class BookGeneralInfoDTO
    {
        public long Id { get; set; }

        public float? AverageRating { get; set; }

        public string? Description { get; set; }

        public long? NumPages { get; set; }

        public short? PublicationDay { get; set; }

        public short? PublicationMonth { get; set; }

        public short? PublicationYear { get; set; }

        public string? ImageUrl { get; set; }

        public long? RatingsCount { get; set; }

        public string? Title { get; set; }

        public string? TitleWithoutSeries { get; set; }

        public double? Price { get; set; }

        public double? Availability { get; set; }

        public float? DiscountPercentage { get; set; }

        public string? AuthorName { get; set; }
    }
}
