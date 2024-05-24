using BookCatalog.API.Model;
using BookCatalog.API.Queries.DTOs;
using BookCatalog.API.Queries.Mappers;
using BookCatalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.API.Controllers
{
    [Route("api/v1/book-reviews")]
    [ApiController]
    public class BookReviewsController : ControllerBase
    {
        private IRepository<BookReview> _reviewsRepository;
        private readonly ILogger<BookReviewsController> _logger; // Inject ILogger

        public BookReviewsController(IRepository<BookReview> reviewsRepository, ILogger<BookReviewsController> logger)
        {
            this._reviewsRepository = reviewsRepository;
            this._logger = logger;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateBookReviewAsync([FromBody] BookReviewDTO bookReviewDTO)
        {
            var bookReview = BookMapper.ToBookReview(bookReviewDTO);

            await _reviewsRepository.AddAsync(bookReview);
            await _reviewsRepository.SaveChangesAsync();

            return Ok("Book review created successfully");
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetBookReviewByUser(
            [FromRoute] Guid userId, 
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10)
        {
            var itemsOnPageQuery = await _reviewsRepository.FindAsync(
                r => r.UserId == userId,
                pageIndex,
                pageSize);

            if (itemsOnPageQuery.Data == null || itemsOnPageQuery.Data.Count == 0)
            {
                return NotFound("Book reviews not found");
            }
            else
            {
                return Ok(new PaginatedItems<BookReview>(
                    itemsOnPageQuery.PageIndex,
                    itemsOnPageQuery.PageSize,
                    itemsOnPageQuery.TotalItems,
                    itemsOnPageQuery.Data));
            }
        }

        [HttpGet("book/{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetBookReviewByBook(
            [FromRoute] int bookId,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10)
        {
            var itemsOnPageQuery = await _reviewsRepository.FindAsync(
                r => r.BookId == bookId,
                pageIndex,
                pageSize);

            if (itemsOnPageQuery.Data == null || itemsOnPageQuery.Data.Count == 0)
            {
                return NotFound("Book reviews not found");
            }
            else
            {
                return Ok(new PaginatedItems<BookReview>(
                    itemsOnPageQuery.PageIndex,
                    itemsOnPageQuery.PageSize,
                    itemsOnPageQuery.TotalItems,
                    itemsOnPageQuery.Data));
            }
        }

        [HttpPatch("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateBookReview([FromBody] BookReviewDTO dto)
        {
            var existingReviewPage = await _reviewsRepository.FindAsync(
                b => b.UserId == dto.UserId && b.BookId == dto.BookId,
                0,
                5);

            var existingReview = existingReviewPage.Data.ElementAt(0);

            if (existingReview == null)
            {
                return NotFound("Review not found for update");
            }

            await _reviewsRepository.Update(BookMapper.ToBookReview(dto));
            await _reviewsRepository.SaveChangesAsync();

            return Ok("Review updated successfully");
        }

        [HttpDelete("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteBookReview([FromQuery] Guid userId, [FromQuery] int bookId)
        {
            var existingReviewPage = await _reviewsRepository.FindAsync(
                b => b.UserId == userId && b.BookId == bookId,
                0,
                5);

            var existingReview = existingReviewPage.Data.ElementAt(0);

            if (existingReview == null)
            {
                return NotFound("Review not found for deletion");
            }

            await _reviewsRepository.Remove(new BookReview { BookId = bookId, UserId = userId});
            await _reviewsRepository.SaveChangesAsync();

            return Ok("Review deleted successfully");
        }
    }
}
