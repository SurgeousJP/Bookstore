using BookCatalog.API.Model;

namespace BookCatalog.API.DTOs
{
    public class CreateGenreDTO
    {
        public string Name { get; set; }

        public Genre ToGenre() => new Genre { Name = Name };
    }
}
