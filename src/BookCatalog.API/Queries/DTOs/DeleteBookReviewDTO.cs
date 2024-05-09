namespace BookCatalog.API.Queries.DTOs
{
    public class DeleteBookReviewDTO
    {
        public Guid UserId { get; set; }

        public long BookId { get; set; }
    }
}
