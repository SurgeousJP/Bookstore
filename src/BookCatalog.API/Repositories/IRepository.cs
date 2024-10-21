using BookCatalog.API.Model;
using System.Linq.Expressions;

namespace BookCatalog.API.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T> GetItemByIdAsync(long id);
        Task<PaginatedItems<T>> SearchAsync(
            string searchWord,
            Expression<Func<T, bool>> predicate,
            bool isDescending,
            int pageIndex = 0,
            int pageSize = 10);
        Task<PaginatedItems<T>> GetAllAsync(int pageIndex = 0, int pageSize = 10);
        Task<PaginatedItems<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            bool isDescending,
            int pageIndex = 0,
            int pageSize = 10);
        Task Update(T entity);
        Task Remove(T entity);
        Task SaveChangesAsync();
        Task<long> LongCountAsync();
        Task<List<string>> GetConstants();
    }
}
