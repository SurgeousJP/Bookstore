using System.Linq.Expressions;

namespace BookCatalog.API.Extensions
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> New<T>(bool defaultExpression)
        {
            return f => defaultExpression;
        }
    }

}
