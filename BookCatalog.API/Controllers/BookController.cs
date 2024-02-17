using BookCatalog.API.Model;
using Microsoft.AspNetCore.Mvc;
using BookCatalog.API.Repositories;
using BookCatalog.API.Queries.DTOs;
using BookCatalog.API.Queries.Mappers;

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
                    itemsOnPageQuery.Data.Select(
                        book => BookMapper.ToBookGeneralInfoDTO(book))
                    .ToList()));
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
                    itemsOnPageQuery.Data.Select(
                        book => BookMapper.ToBookGeneralInfoDTO(book))
                    .ToList()));
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
                    itemsOnPageQuery.Data.Select(
                        book => BookMapper.ToBookGeneralInfoDTO(book)).ToList())); 
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

            var book = await bookRepository.GetItemByIdAsync(id);

            if (book == null)
            {
                return NotFound("Book not found");
            }

            return Ok(BookMapper.ToBookDetailDTO(book));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBookAsync([FromBody] CreateBookDTO bookInfo)
        {
            Book book = BookMapper.ToBookFromCreateBookDTO(bookInfo);

            await bookRepository.AddAsync(book);
            await bookRepository.SaveChangesAsync();

            return Ok("Book created successfully");
        }

        [HttpPatch("update")]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBookAsync([FromBody] BookDetailDTO book)
        {
            try
            {
                await bookRepository.Update(BookMapper.ToBookFromBookDetailDTO(book));
                await bookRepository.SaveChangesAsync();
                return Ok("Book updated successfully");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete("delete")]
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
