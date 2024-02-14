using BookCatalog.API.Model;

namespace BookCatalog.API.DTOs
{
    public class CreateBookDTO
    {
        public string LanguageCode { get; set; } = null!;

        public float? AverageRating { get; set; }

        public string? Description { get; set; }

        public long? NumPages { get; set; }

        public short? PublicationDay { get; set; }

        public short? PublicationMonth { get; set; }

        public short? PublicationYear { get; set; }

        public string? Isbn13 { get; set; }

        public string? Url { get; set; }

        public string? ImageUrl { get; set; }

        public long? RatingsCount { get; set; }

        public string? Title { get; set; }

        public string? TitleWithoutSeries { get; set; }

        public double? Price { get; set; }

        public double? Availability { get; set; }

        public string? Dimensions { get; set; }

        public float? DiscountPercentage { get; set; }

        public double? ItemWeight { get; set; }

        public long FormatId { get; set; }

        public long PublisherId { get; set; }

        public string AuthorName { get; set; }

        public Book ToBook() => new Book
        {
            LanguageCode = this.LanguageCode,
            AverageRating = this.AverageRating,
            Description = this.Description,
            NumPages = this.NumPages,
            PublicationDay = this.PublicationDay,
            PublicationMonth = this.PublicationMonth,
            PublicationYear = this.PublicationYear,
            Isbn13 = this.Isbn13,
            Url = this.Url,
            ImageUrl = this.ImageUrl,
            RatingsCount = this.RatingsCount,
            Title = this.Title,
            TitleWithoutSeries = this.TitleWithoutSeries,
            Price = this.Price,
            Availability = this.Availability,
            Dimensions = this.Dimensions,
            DiscountPercentage = this.DiscountPercentage,
            ItemWeight = this.ItemWeight,
            FormatId = this.FormatId,
            AuthorName = this.AuthorName,
            PublisherId = this.PublisherId,
        };
    }
}
