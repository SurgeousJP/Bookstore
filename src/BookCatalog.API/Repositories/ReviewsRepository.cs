using BookCatalog.API.Infrastructure;
using BookCatalog.API.Model;

namespace BookCatalog.API.Repositories
{
    public class ReviewsRepository : GenericRepository<BookReview>
    {
        public ReviewsRepository(BookContext context) : base(context)
        {
        }
    }
}
