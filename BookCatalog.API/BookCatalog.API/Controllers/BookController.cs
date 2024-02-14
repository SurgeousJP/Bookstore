using BookCatalog.API.Model;
using BookCatalog.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using BookCatalog.API.Infrastructure.Repositories;

namespace BookCatalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IRepository<Book> bookRepository;

        public BookController(IRepository<Book> bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        [HttpGet("search_query")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> SearchBookAsync(
            [FromQuery] string searchWord,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageIndex = 0)
        {
            var itemsOnPageQuery = await bookRepository.SearchAsync(
                searchWord,
                pageIndex,
                pageSize
                );

            if (itemsOnPageQuery.Data == null || itemsOnPageQuery.Data.Count == 0)
            {
                return NotFound("Books not found");
            }
            else
            {
                return Ok(new PaginatedItems<BookGeneralInfoDTO>(
                    itemsOnPageQuery.PageIndex,
                    itemsOnPageQuery.PageSize,
                    itemsOnPageQuery.TotalItems,
                    itemsOnPageQuery.Data.Select(book => new BookGeneralInfoDTO()
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
                    }).ToList()));
            }
        }

        [HttpGet("items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetBooksAsync(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10 
            )
        {
            var totalItems = await bookRepository.LongCountAsync();

            var itemsOnPageQuery = await bookRepository.GetAllAsync(pageIndex, pageSize);

            if (itemsOnPageQuery.Data == null || itemsOnPageQuery.Data.Count == 0)
            {
                return NotFound("Books not found");
            }
            else
            {
                return Ok(new PaginatedItems<BookGeneralInfoDTO>(
                    itemsOnPageQuery.PageIndex,
                    itemsOnPageQuery.PageSize,
                    itemsOnPageQuery.TotalItems,
                    itemsOnPageQuery.Data.Select(book => new BookGeneralInfoDTO()
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
                    }).ToList()));
            }
        }

        [HttpGet("items/filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetBooksByFilterAsync(
            [FromQuery] BookFilter filter,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10
            )
        {      

            var itemsOnPageQuery = await bookRepository.FindAsync(
               BookFilter.BuildFilterPredicate(filter),
                pageIndex, 
                pageSize);

            if (itemsOnPageQuery.Data == null || itemsOnPageQuery.Data.Count == 0)
            {
                return NotFound("Books not found");
            }
            else
            {
                return Ok(new PaginatedItems<BookGeneralInfoDTO>(
                    itemsOnPageQuery.PageIndex,
                    itemsOnPageQuery.PageSize,
                    itemsOnPageQuery.TotalItems,
                    itemsOnPageQuery.Data.Select(book => new BookGeneralInfoDTO()
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
                    }).ToList()));
            }
        }

        [HttpGet("item/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetBookByIdAsync([FromRoute] int id = 1)
        {
            if (id <= 0)
            {
                return BadRequest("Id is not valid");
            }

            Book book = await bookRepository.GetItemByIdAsync(id);

            if (book == null)
            {
                return NotFound("Book not found");
            }

            return Ok(book);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookDTO bookInfo)
        {
            Book book = bookInfo.ToBook();
            await bookRepository.AddAsync(book);
            await bookRepository.SaveChangesAsync();
            return Ok(book);
        }

        [HttpPut("update")]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBookAsync([FromBody] Book book)
        {
            var existingBook = await bookRepository.GetItemByIdAsync(book.Id);

            if (existingBook != null)
            {
                return NotFound("Book for update not found");
            }

            bookRepository.Update(book);
            await bookRepository.SaveChangesAsync();
            return Ok(existingBook);
        }

        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteBookAsync([FromQuery] int id)
        {
            var existingBook = await bookRepository.GetItemByIdAsync(id);

            if (existingBook == null)
            {
                return NotFound("Book for delete not found");
            }

            bookRepository.Remove(new Book { Id = id });
            await bookRepository.SaveChangesAsync();

            return Ok("Book deleted");
        }
    }
}
