using BookCatalog.API.Model;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.API.Infrastructure.Repositories
{
    public class GenreRepository : GenericRepository<Genre>
    {
        public GenreRepository(PostgresContext context) : base(context)
        {
        }
    }
}
