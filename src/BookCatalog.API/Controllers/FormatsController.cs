using BookCatalog.API.Model;
using BookCatalog.API.Queries.DTOs;
using BookCatalog.API.Queries.Mappers;
using BookCatalog.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles="Admin")]
    public class FormatsController : ControllerBase
    {
        private IRepository<BookFormat> formatRepository;

        public FormatsController(IRepository<BookFormat> formatRepository)
        {
            this.formatRepository = formatRepository;
        }

        [AllowAnonymous]
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetFormatsAsync(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10)
        {
            var totalFormats = await formatRepository.LongCountAsync();

            var formatsInPage = await formatRepository.GetAllAsync(pageIndex, pageSize);

            if (formatsInPage == null)
            {
                return NotFound("Formats not found");
            }
            else
            {
                return Ok(new PaginatedItems<BookFormat>(
                    formatsInPage.PageIndex,
                    formatsInPage.PageSize,
                    formatsInPage.TotalItems,
                    formatsInPage.Data));
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateFormatAsync([FromBody] CreateFormatDTO createFormat)
        {
            BookFormat format = BookMapper.ToFormat(createFormat);
            await formatRepository.AddAsync(format);
            await formatRepository.SaveChangesAsync();

            return Ok(format);
        }

        [HttpPatch("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateFormatAsync([FromBody] BookFormat updateFormat)
        {
            var existingFormat = await formatRepository.GetItemByIdAsync(updateFormat.Id);

            if (existingFormat == null)
            {
                return NotFound("Format not found update");
            }

            await formatRepository.Update(updateFormat);
            await formatRepository.SaveChangesAsync();

            return Ok("Format updated successfully");
        }

        [HttpDelete("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteFormatAsync([FromQuery] int id)
        {
            var existingFormat = await formatRepository.GetItemByIdAsync(id);

            if (existingFormat == null)
            {
                return NotFound("Format not found for delete");
            }

            await formatRepository.Remove(new BookFormat { Id = id });

            return Ok("Format deleted successfully");
        }

    }
}
