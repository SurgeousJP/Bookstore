using BookCatalog.API.Model;

namespace BookCatalog.API.Infrastructure.Repositories
{
    public class FormatRepository : GenericRepository<BookFormat>
    {
        public FormatRepository(PostgresContext context) : base(context)
        {
        }
    }
}
