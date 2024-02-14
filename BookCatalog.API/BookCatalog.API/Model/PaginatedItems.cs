namespace BookCatalog.API.Model
{
    public class PaginatedItems<TEntity>(int pageIndex, int pageSize, long totalItems, IEnumerable<TEntity> data) where TEntity : class
    {
        public int PageIndex { get; } = pageIndex;

        public int PageSize { get; } = pageSize;

        public long TotalItems { get; } = totalItems;

        public IEnumerable<TEntity> Data { get; } = data;
    }
}
