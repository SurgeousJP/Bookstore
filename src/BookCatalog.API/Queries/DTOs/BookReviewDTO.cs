using System.ComponentModel.DataAnnotations;

namespace BookCatalog.API.Queries.DTOs
{
    public class BookReviewDTO
    {
        public Guid UserId { get; set; }

        public long BookId { get; set; }

        public string? Username { get; set; }

        public string? UserProfileImage { get; set; }

        public string? Comment { get; set; }

        public decimal? RatingPoint { get; set; }
    }
}
