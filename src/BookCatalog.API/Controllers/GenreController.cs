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
    public class GenreController : ControllerBase
    {
        private IRepository<Genre> genreRepository;

        public GenreController(IRepository<Genre> genreRepository)
        {
            this.genreRepository = genreRepository;
        }

        [HttpGet("items")]
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
                return Ok(new {
                    totalGenres = totalGenres,
                    genres = genresInPage
                });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateGenreAsync([FromBody] CreateGenreDTO createGenre)
        {
            Genre genre = BookMapper.ToGenre(createGenre);
            await genreRepository.AddAsync(genre);
            await genreRepository.SaveChangesAsync();

            return Ok(genre);
        }

        [HttpPatch("update")] 
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

            genreRepository.Update(updateGenre);
            await genreRepository.SaveChangesAsync();

            return Ok("Genre updated successfully");
        }

        [HttpDelete("delete")]
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

            genreRepository.Remove(new Genre { Id = id });

            return Ok("Genre deleted successfully");
        }

    }
}
