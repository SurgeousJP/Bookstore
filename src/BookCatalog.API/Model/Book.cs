using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookCatalog.API.Model;

public partial class Book
{
    public long Id { get; set; }

    public string? LanguageCode { get; set; } = null!;

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

    public string? AuthorName { get; set; }

    public virtual ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();

    public virtual BookFormat Format { get; set; } = null!;

    public virtual BookPublisher Publisher { get; set; } = null!;

    public virtual ICollection<BookReview> BookReviews { get; set; } = new List<BookReview>();
}
