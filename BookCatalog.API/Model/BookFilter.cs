using BookCatalog.API.Extensions;
using System.Linq.Expressions;

namespace BookCatalog.API.Model
{
    public class BookFilter
    {
        public string? Title { get; set; }
        public string? TitleWithoutSeries { get; set; }
        public string? Description { get; set; }
        public string[]? LanguageCodes { get; set; }
        public int[]? FormatIds { get; set; }
        public string? AuthorName { get; set; }
        public int[]? GenreIds { get; set; }

        public static Expression<Func<Book, bool>> BuildFilterPredicate(BookFilter filter)
        {
            Expression<Func<Book, bool>> filterExpression = PredicateBuilderExtension.True<Book>();
            if (filter.Title != null && filter.Title.Length > 0)
            {
                filterExpression = filterExpression.And(book => book.Title.Contains(filter.Title));
            }
            if (filter.TitleWithoutSeries != null && filter.TitleWithoutSeries.Length > 0)
            {
                filterExpression = filterExpression.And(book => book.TitleWithoutSeries.Contains(filter.TitleWithoutSeries));
            }
            if (filter.Description != null && filter.Description.Length > 0)
            {
                filterExpression = filterExpression.And(book => book.Description.Contains(filter.Description));
            }
            if (filter.LanguageCodes != null && filter.LanguageCodes.Length > 0)
            {
                filterExpression = filterExpression.And(book => filter.LanguageCodes.Any(filter => filter == book.LanguageCode));
            }
            if (filter.FormatIds != null && filter.FormatIds.Length > 0)
            {
                filterExpression = filterExpression.And(book => filter.FormatIds.Any(filter => filter == book.FormatId));
            }
            if (filter.AuthorName != null && filter.AuthorName.Length > 0)
            {
                filterExpression = filterExpression.And(book => book.AuthorName.Contains(filter.AuthorName));
            }
            if (filter.GenreIds != null && filter.GenreIds.Length > 0)
            {
                filterExpression = filterExpression.And(book => filter.GenreIds.Any(filter => book.BookGenres.Any(bg => bg.GenreId == filter)));
            }
            return filterExpression;
        }
    }}
