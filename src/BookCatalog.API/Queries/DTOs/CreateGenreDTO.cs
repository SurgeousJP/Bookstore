using BookCatalog.API.Model;

namespace BookCatalog.API.Queries.DTOs
{
    public class CreateGenreDTO
    {
        public required string Name { get; set; }

        public string ImageURL { get; set; }
    }
}
