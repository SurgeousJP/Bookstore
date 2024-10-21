using BookCatalog.API.Model;
using BookCatalog.API.Queries.DTOs;
using BookCatalog.API.Queries.Mappers;
using BookCatalog.API.Repositories;
using Catalog.API.Extensions;
using EventBus.Messaging.Events;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace BookCatalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private IRepository<Book> bookRepository;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly ILogger<BooksController> logger; // Inject ILogger

        public BooksController(IRepository<Book> bookRepository, IPublishEndpoint publishEndpoint, ILogger<BooksController> logger)
        {
            this.bookRepository = bookRepository;
            this.publishEndpoint = publishEndpoint;
            this.logger = logger;
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> SearchBookAsync(
            [FromQuery] string searchWord,
            [FromQuery] BookFilter filter,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool isPriceAscend = true,
            [FromQuery] double startPrice = 0,
            [FromQuery] double endPrice = 0
            )
        {
            var predicate = BookFilter.BuildFilterPredicate(filter);
            Expression<Func<Book, bool>> pricePredicate = book => book.Price >= startPrice && book.Price <= endPrice;

            if (startPrice > 0 && endPrice > 0 && startPrice < endPrice)
            {
                predicate = predicate.And(pricePredicate);
            }

            var itemsOnPageQuery = await bookRepository.SearchAsync(
                searchWord,
                predicate,
                !isPriceAscend,
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
                    itemsOnPageQuery.Data.Select(
                        book => BookMapper.ToBookGeneralInfoDTO(book))
                    .ToList()));
            }
        }

        [HttpGet("lang-codes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAllLanguageCodeAsync()
        {
            var langCodes = await bookRepository.GetConstants();

            if (langCodes == null || langCodes.Count == 0)
            {
                return NotFound("Language codes not found");
            }

            return Ok(langCodes);
        }

        [HttpGet("")]
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
                    itemsOnPageQuery.Data.Select(
                        book => BookMapper.ToBookGeneralInfoDTO(book))
                    .ToList()));
            }
        }

        [HttpGet("filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetBooksByFilterAsync(
            [FromQuery] BookFilter filter,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool isPriceAscend = true)
        {
            var predicate = BookFilter.BuildFilterPredicate(filter);
            Expression<Func<Book, object>> sortPredicate = book => book.Price;

            var itemsOnPageQuery = await bookRepository.FindAsync(
                predicate,
                !isPriceAscend,
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
                    itemsOnPageQuery.Data.Select(
                        book => BookMapper.ToBookGeneralInfoDTO(book)).ToList()));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetBookByIdAsync([FromRoute] int id = 1)
        {
            if (id <= 0)
            {
                return BadRequest("Id is not valid");
            }

            var book = await bookRepository.GetItemByIdAsync(id);

            if (book == null)
            {
                return NotFound("Book not found");
            }

            return Ok(BookMapper.ToBookDetailDTO(book));
        }

        //[Authorize(Roles="Admin")]
        [HttpPost("")]
        public async Task<IActionResult> CreateBookAsync([FromBody] CreateBookDTO bookInfo)
        {
            Book book = BookMapper.ToBookFromCreateBookDTO(bookInfo);

            await bookRepository.AddAsync(book);
            await bookRepository.SaveChangesAsync();

            return Ok("Book created successfully");
        }

        //[Authorize(Roles = "Admin")]
        [Authorize]
        [HttpPatch("")]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBookAsync([FromBody] BookDetailDTO book)
        {
            try
            {
                var currentBook = await bookRepository.GetItemByIdAsync(book.Id);

                if (currentBook == null)
                {
                    return BadRequest("Book for updated not found");
                }

                if (book.Price != currentBook.Price)
                {
                    logger.LogInformation($"Send data book id : {book.Id} new price: {book.Price}");

                    var eventMessage = new ProductPriceUpdateEvent
                    {
                        BookId = (int)book.Id,
                        NewPrice = (double)book.Price
                    };

                    logger.LogInformation("Beginning event update product price");
                    await publishEndpoint.Publish(eventMessage);
                }

                await bookRepository.Update(BookMapper.ToBookFromBookDetailDTO(book));
                await bookRepository.SaveChangesAsync();
                return Ok("Book updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //[Authorize(Roles = "Admin")]
        [Authorize]
        [HttpDelete("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteBookAsync([FromQuery] int id)
        {
            try
            {
                await bookRepository.Remove(new Book { Id = id });
                await bookRepository.SaveChangesAsync();
                return Ok("Book deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
