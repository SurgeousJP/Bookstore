using BookCatalog.API.Infrastructure;
using BookCatalog.API.Model;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.API.Repositories
{
    public class GenreRepository : GenericRepository<Genre>
    {
        public GenreRepository(BookContext context) : base(context)
        {
        }
    }
}
