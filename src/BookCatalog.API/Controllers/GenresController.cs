using BookCatalog.API.Model;
using BookCatalog.API.Queries.DTOs;
using BookCatalog.API.Queries.Mappers;
using BookCatalog.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.API.Controllers
{
    [Authorize(Roles="Admin")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private IRepository<Genre> genreRepository;

        public GenresController(IRepository<Genre> genreRepository)
        {
            this.genreRepository = genreRepository;
        }

        [AllowAnonymous]
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetGenresAsync(
            [FromQuery]int pageIndex = 0, 
            [FromQuery]int pageSize = 10){
            var totalGenres = await genreRepository.LongCountAsync();

            var genresInPage = await genreRepository.GetAllAsync(pageIndex, pageSize);

            if (genresInPage == null)
            {
                return NotFound("Genres not found");
            }
            else
            {
                return Ok(new PaginatedItems<Genre>(
                    genresInPage.PageIndex,
                    genresInPage.PageSize,
                    genresInPage.TotalItems,
                    genresInPage.Data));
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateGenreAsync([FromBody] CreateGenreDTO createGenre)
        {
            Genre genre = BookMapper.ToGenre(createGenre);
            await genreRepository.AddAsync(genre);
            await genreRepository.SaveChangesAsync();

            return Ok(genre);
        }

        [HttpPatch("")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateGenreAsync([FromBody] Genre updateGenre)
        {
            var existingGenre = await genreRepository.GetItemByIdAsync(updateGenre.Id);

            if (existingGenre == null)
            {
                return NotFound("Genre not found update");
            }

            await genreRepository.Update(updateGenre);
            await genreRepository.SaveChangesAsync();

            return Ok("Genre updated successfully");
        }

        [HttpDelete("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteGenreAsync([FromQuery]int id)
        {
            var existingGenre = await genreRepository.GetItemByIdAsync(id);

            if (existingGenre == null)
            {
                return NotFound("Genre not found for delete");
            }

            await genreRepository.Remove(new Genre { Id = id });

            return Ok("Genre deleted successfully");
        }

    }
}
