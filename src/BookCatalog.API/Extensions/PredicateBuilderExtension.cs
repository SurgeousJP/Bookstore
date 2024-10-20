using BookCatalog.API.Extensions;
using System.Linq.Expressions;
namespace Catalog.API.Extensions
{
    public static class PredicateBuilderExtension
    {
        public static Expression<Func<T, bool>> True<T>() { return param => true; }

        public static Expression<Func<T, bool>> False<T>() { return param => false; }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());

            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> BuildPredicate<T>(Dictionary<string, string> filters)
        {
            // Start with a predicate that always returns true (no filters)
            var predicate = PredicateBuilder.New<T>(true);

            foreach (var filter in filters)
            {
                predicate = predicate.And(BuildCondition<T>(filter.Key, filter.Value));
            }

            return predicate;
        }


        public static Expression<Func<T, bool>> BuildCondition<T>(string propertyName, string value)
        {
            var parameter = Expression.Parameter(typeof(T), "p");
            var property = Expression.Property(parameter, propertyName);

            // Check if the property is of type string
            if (property.Type == typeof(string))
            {
                // Apply the "Contains" method for string types
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var condition = Expression.Call(property, containsMethod, Expression.Constant(value));
                return Expression.Lambda<Func<T, bool>>(condition, parameter);
            }
            else
            {
                // Handle other types with equality check
                object convertedValue = Convert.ChangeType(value, property.Type);
                var condition = Expression.Equal(property, Expression.Constant(convertedValue));
                return Expression.Lambda<Func<T, bool>>(condition, parameter);
            }
        }


    }
}
