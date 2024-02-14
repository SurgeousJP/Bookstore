namespace BookCatalog.API.Model
{
    public class PaginatedItems<TEntity>(int pageIndex, int pageSize, long totalItems, ICollection<TEntity> data) where TEntity : class
    {
        public int PageIndex { get; } = pageIndex;

        public int PageSize { get; } = pageSize;

        public long TotalItems { get; } = totalItems;

        public ICollection<TEntity> Data { get; } = data;
    }
}
