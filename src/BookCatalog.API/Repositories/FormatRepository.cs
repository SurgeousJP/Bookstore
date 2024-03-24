using BookCatalog.API.Infrastructure;
using BookCatalog.API.Model;

namespace BookCatalog.API.Repositories
{
    public class FormatRepository : GenericRepository<BookFormat>
    {
        public FormatRepository(BookContext context) : base(context)
        {
        }
    }
}
