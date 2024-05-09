using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookCatalog.API.Model
{
    public class BookReview
    {
        [Key]
        public Guid UserId { get; set; }
        [Key]
        public long BookId { get; set; }

        public string? Username { get; set; }

        public string? UserProfileImage { get; set; }

        public string? Comment { get; set; }

        public decimal? RatingPoint { get; set; }

        public DateOnly? CreationDate { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public virtual Book Book { get; set; } = null!;
    }
}
