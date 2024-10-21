using BookCatalog.API.Infrastructure;
using BookCatalog.API.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookCatalog.API.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected BookContext context;

        public GenericRepository(BookContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual async Task AddAsync(T entity)
        {
            await context.AddAsync(entity);
        }

        public virtual async Task<PaginatedItems<T>> SearchAsync(
            string searchWord,
            Expression<Func<T, bool>> predicate,
            bool isDescending,
            int pageIndex = 0,
            int pageSize = 10
            )
        {
            throw new NotImplementedException();
        }

        public async virtual Task<PaginatedItems<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            bool isDescending,
            int pageIndex = 0,
            int pageSize = 10)
        {
            var query = context.Set<T>().AsQueryable().Where(predicate);

            var totalItems = await query.LongCountAsync();

            if (pageIndex >= 0 && pageSize > 0)
            {
                query = query.Skip(pageIndex * pageSize).Take(pageSize);
            }

            return new PaginatedItems<T>(pageIndex, pageSize, totalItems, await query.ToListAsync());
        }

        public virtual async Task<T> GetItemByIdAsync(long id)
        {
            return await context.FindAsync<T>(id);
        }

        public virtual async Task<PaginatedItems<T>> GetAllAsync(int pageIndex = 0, int pageSize = 0)
        {
            var query = context.Set<T>().AsQueryable();

            var totalItems = await query.LongCountAsync();

            if (pageIndex >= 0 && pageSize > 0)
            {
                query = query.Skip(pageIndex * pageSize).Take(pageSize);
            }

            return new PaginatedItems<T>(pageIndex, pageSize, totalItems, await query.ToListAsync());
        }

        public virtual async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public virtual async Task Update(T entity)
        {
            context.ChangeTracker.Clear();
            context.Update(entity);
        }

        public virtual async Task Remove(T entity)
        {
            context.ChangeTracker.Clear();
            context.Remove(entity);
        }

        public async Task<long> LongCountAsync()
        {
            return await context.Set<T>().LongCountAsync();
        }

        public virtual async Task<List<string>> GetConstants()
        {
            return null;
        }
    }
}
