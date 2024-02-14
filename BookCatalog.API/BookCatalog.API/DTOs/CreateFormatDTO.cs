using BookCatalog.API.Model;

namespace BookCatalog.API.DTOs
{
    public class CreateFormatDTO
    {
        public string Name { get; set; }

        public BookFormat ToFormat() => new BookFormat { Name = Name };
    }
}
