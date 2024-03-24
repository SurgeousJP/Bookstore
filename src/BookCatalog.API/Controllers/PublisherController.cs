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
    public class PublisherController : ControllerBase
    {
        private IRepository<BookPublisher> publisherRepository;

        public PublisherController(IRepository<BookPublisher> publisherRepository)
        {
            this.publisherRepository = publisherRepository;
        }

        [HttpGet("items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetPublishersAsync(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10)
        {
            var totalPublishers = await publisherRepository.LongCountAsync();

            var publishersInPage = await publisherRepository.GetAllAsync(pageIndex, pageSize);

            if (publishersInPage == null)
            {
                return NotFound("Publishers not found");
            }
            else
            {
                return Ok(new
                {
                    totalPublishers = totalPublishers,
                    publishers = publishersInPage
                });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePublisherAsync([FromBody] CreatePublisherDTO createPublisher)
        {
            BookPublisher publisher = BookMapper.ToBookPublisher(createPublisher);
            await publisherRepository.AddAsync(publisher);
            await publisherRepository.SaveChangesAsync();

            return Ok(publisher);
        }

        [HttpPatch("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdatePublisherAsync([FromBody] BookPublisher updatePublisher)
        {
            var existingPublisher = await publisherRepository.GetItemByIdAsync(updatePublisher.Id);

            if (existingPublisher == null)
            {
                return NotFound("Publisher not found update");
            }

            publisherRepository.Update(updatePublisher);
            await publisherRepository.SaveChangesAsync();

            return Ok("Publisher updated successfully");
        }

        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeletePublisherAsync([FromQuery] int id)
        {
            var existingPublisher = await publisherRepository.GetItemByIdAsync(id);

            if (existingPublisher == null)
            {
                return NotFound("Publisher not found for delete");
            }

            publisherRepository.Remove(new BookPublisher { Id = id });

            return Ok("Publisher deleted successfully");
        }

    }
}
