using BookCatalog.API.Infrastructure;
using BookCatalog.API.Model;

namespace BookCatalog.API.Repositories
{
    public class PublisherRepository : GenericRepository<BookPublisher>
    {
        public PublisherRepository(BookContext context) : base(context)
        {
        }
    }
}
