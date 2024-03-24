using BookCatalog.API.Model;
using BookCatalog.API.Queries.DTOs;
using BookCatalog.API.Queries.Mappers;
using BookCatalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormatController : ControllerBase
    {
        private IRepository<BookFormat> formatRepository;

        public FormatController(IRepository<BookFormat> formatRepository)
        {
            this.formatRepository = formatRepository;
        }

        [HttpGet("items")]
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
                return Ok(new
                {
                    totalFormats = totalFormats,
                    formats = formatsInPage
                });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateFormatAsync([FromBody] CreateFormatDTO createFormat)
        {
            BookFormat format = BookMapper.ToFormat(createFormat);
            await formatRepository.AddAsync(format);
            await formatRepository.SaveChangesAsync();

            return Ok(format);
        }

        [HttpPatch("update")]
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

            formatRepository.Update(updateFormat);
            await formatRepository.SaveChangesAsync();

            return Ok("Format updated successfully");
        }

        [HttpDelete("delete")]
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

            formatRepository.Remove(new BookFormat { Id = id });

            return Ok("Format deleted successfully");
        }

    }
}
