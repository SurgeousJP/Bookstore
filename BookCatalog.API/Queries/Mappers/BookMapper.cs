using BookCatalog.API.Model;
using BookCatalog.API.Queries.DTOs;
using System;
using System.Xml.Linq;

namespace BookCatalog.API.Queries.Mappers
{
    public static class BookMapper

    {
        public static void MapBookToBook (Book descBook, Book book)
        {
            descBook.LanguageCode = book.LanguageCode;
            descBook.AverageRating = book.AverageRating;
            descBook.Description = book.Description;
            descBook.NumPages = book.NumPages;
            descBook.PublicationDay = book.PublicationDay;
            descBook.PublicationMonth = book.PublicationMonth;
            descBook.PublicationYear = book.PublicationYear;
            descBook.Isbn13 = book.Isbn13;
            descBook.ImageUrl = book.ImageUrl;
            descBook.RatingsCount = book.RatingsCount;
            descBook.Title = book.Title;
            descBook.TitleWithoutSeries = book.TitleWithoutSeries;
            descBook.Price = book.Price;
            descBook.Availability = book.Availability;
            descBook.Dimensions = book.Dimensions;
            descBook.DiscountPercentage = book.DiscountPercentage;
            descBook.ItemWeight = book.ItemWeight;
            descBook.FormatId = book.FormatId;
            descBook.AuthorName = book.AuthorName;
            descBook.PublisherId = book.PublisherId;
            descBook.BookGenres.Clear();
            foreach (var genre in book.BookGenres)
            {
                descBook.BookGenres.Add(new BookGenre { GenreId = genre.GenreId });
            }
        }
        
        public static Book ToBookFromBookDetailDTO(BookDetailDTO book) => new Book
        {
            Id = book.Id,
            LanguageCode = book.LanguageCode,
            AverageRating = book.AverageRating,
            Description = book.Description,
            NumPages = book.NumPages,
            PublicationDay = book.PublicationDay,
            PublicationMonth = book.PublicationMonth,
            PublicationYear = book.PublicationYear,
            Isbn13 = book.Isbn13,
            ImageUrl = book.ImageUrl,
            RatingsCount = book.RatingsCount,
            Title = book.Title,
            TitleWithoutSeries = book.TitleWithoutSeries,
            Price = book.Price,
            Availability = book.Availability,
            Dimensions = book.Dimensions,
            DiscountPercentage = book.DiscountPercentage,
            ItemWeight = book.ItemWeight,
            FormatId = book.FormatId,
            AuthorName = book.AuthorName,
            PublisherId = book.PublisherId,
            BookGenres = book.BookGenres.Select(genre => new BookGenre { 
                BookId = book.Id,
                GenreId = genre.Id,
            }).ToList(),
        };

        public static Book ToBookFromCreateBookDTO(CreateBookDTO book)
        {
            Book b = new()
            {
                LanguageCode = book.LanguageCode,
                AverageRating = book.AverageRating,
                Description = book.Description,
                NumPages = book.NumPages,
                PublicationDay = book.PublicationDay,
                PublicationMonth = book.PublicationMonth,
                PublicationYear = book.PublicationYear,
                Isbn13 = book.Isbn13,
                ImageUrl = book.ImageUrl,
                RatingsCount = book.RatingsCount,
                Title = book.Title,
                TitleWithoutSeries = book.TitleWithoutSeries,
                Price = book.Price,
                Availability = book.Availability,
                Dimensions = book.Dimensions,
                DiscountPercentage = book.DiscountPercentage,
                ItemWeight = book.ItemWeight,
                FormatId = book.FormatId,
                AuthorName = book.AuthorName,
                PublisherId = book.PublisherId,
            };

            foreach (var genre in book.Genres)
            {
                b.BookGenres.Add(new BookGenre { GenreId = genre.Id });
            }

            return b;
        }
        public static BookGeneralInfoDTO ToBookGeneralInfoDTO(Book book)
        {
            return new BookGeneralInfoDTO()
            {
                Id = book.Id,
                Title = book.Title,
                TitleWithoutSeries = book.TitleWithoutSeries,
                AverageRating = book.AverageRating,
                Description = book.Description,
                NumPages = book.NumPages,
                PublicationDay = book.PublicationDay,
                PublicationMonth = book.PublicationMonth,
                PublicationYear = book.PublicationYear,
                ImageUrl = book.ImageUrl,
                Price = book.Price,
                Availability = book.Availability,
                DiscountPercentage = book.DiscountPercentage,
                AuthorName = book.AuthorName,
                RatingsCount = book.RatingsCount
            };
        }

        public static BookDetailDTO ToBookDetailDTO(Book book)
        {
            return new BookDetailDTO
            {
                Id = book.Id,
                Title = book.Title,
                TitleWithoutSeries = book.TitleWithoutSeries,
                AverageRating = book.AverageRating,
                Description = book.Description,
                NumPages = book.NumPages,
                PublicationDay = book.PublicationDay,
                PublicationMonth = book.PublicationMonth,
                PublicationYear = book.PublicationYear,
                ImageUrl = book.ImageUrl,
                Price = book.Price,
                Availability = book.Availability,
                DiscountPercentage = book.DiscountPercentage,
                AuthorName = book.AuthorName,
                RatingsCount = book.RatingsCount,
                LanguageCode = book.LanguageCode,
                Isbn13 = book.Isbn13,
                Dimensions = book.Dimensions,
                ItemWeight = book.ItemWeight,
                BookGenres = book.BookGenres.Select(bg => bg.Genre).ToList(),
                FormatId = book.FormatId,
                FormatName = book.Format.Name,
                PublisherId = book.PublisherId,
                PublisherName = book.Publisher.Name
            };
        }

        public static BookFormat ToFormat(CreateFormatDTO format) 
            => new BookFormat { Name = format.Name };

        public static Genre ToGenre(CreateGenreDTO genre)
            => new Genre { Name = genre.Name };

        public static BookPublisher ToBookPublisher(CreatePublisherDTO publisher) 
            => new BookPublisher { Name = publisher.Name };
    }
}
