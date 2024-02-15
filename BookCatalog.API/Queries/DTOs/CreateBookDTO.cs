using BookCatalog.API.Model;
using System.ComponentModel.DataAnnotations;

namespace BookCatalog.API.Queries.DTOs
{
    public class CreateBookDTO
    {
        [Required(ErrorMessage = "Language code is required")]
        public string LanguageCode { get; set; } = null!;

        [Required(ErrorMessage = "Average rating is required")]
        [Range(0, float.MaxValue, ErrorMessage = "Average rating must be non-negative")]
        public float? AverageRating { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MinLength(1, ErrorMessage = "Description length must be greater than one")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Num pages is required")]
        [Range(1, long.MaxValue, ErrorMessage = "Average rating must be a positive integer")]
        public long? NumPages { get; set; }

        [Required(ErrorMessage = "Publication day is required")]
        public short? PublicationDay { get; set; }

        [Required(ErrorMessage = "Publication month is required")]
        public short? PublicationMonth { get; set; }

        [Required(ErrorMessage = "Publication year is required")]
        public short? PublicationYear { get; set; }

        public string? Isbn13 { get; set; }

        [Required(ErrorMessage = "Image url is required")]

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Ratings count is required")]
        [Range(0, float.MaxValue, ErrorMessage = "Average rating must be non-negative")]
        public long? RatingsCount { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MinLength(1, ErrorMessage = "Title length must be greater than one")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "TitleWithoutSeries is required")]
        [MinLength(1, ErrorMessage = "TitleWithoutSeries length must be greater than one")]
        public string? TitleWithoutSeries { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, float.MaxValue, ErrorMessage = "Price must be non-negative")]
        public double? Price { get; set; }

        [Required(ErrorMessage = "Availability is required")]
        [Range(0, float.MaxValue, ErrorMessage = "Price must be non-negative")]
        public double? Availability { get; set; }

        [Required(ErrorMessage = "Dimensions is required")]
        public string? Dimensions { get; set; }

        [Required(ErrorMessage = "Discount percentage is required")]
        [Range(0, 1, ErrorMessage = "Discount percentage must be between zero and one")]
        public float? DiscountPercentage { get; set; }

        [Required(ErrorMessage = "Item weight is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Item weight must be non-negative")]
        public double? ItemWeight { get; set; }

        [Required(ErrorMessage = "Author name is required")]
        [MinLength(1, ErrorMessage = "Author name length must be greater than one")]
        public string? AuthorName { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one genre is required.")]
        public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

        [Required]
        public long FormatId { get; set; } 

        [Required]
        public long PublisherId { get; set; }
    }
}
