using API.Dtos;
using API.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Tour.Controllers
{
    [ApiController]
    [Route("api/tours")]
    public class TourController : ControllerBase
    {
        private readonly ITourService _tourService;

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        [Authorize(Policy = "authorPolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateTour([FromBody] TourDto tourDto)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(authorId)) return Unauthorized();

            tourDto.AuthorId = authorId;
            var createdTour = await _tourService.CreateTourAsync(tourDto);
            return Ok(createdTour);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTourById(int id)
        {
            try
            {
                var tour = await _tourService.GetTourByIdAsync(id);
                return tour is null ? NotFound() : Ok(tour);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "authorPolicy")]
        [HttpGet("author")]
        public async Task<IActionResult> GetToursByAuthor()
        {
            try
            {
                var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(authorId)) return Unauthorized();

                var tours = await _tourService.GetToursByAuthorAsync(authorId);
                return Ok(tours);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("by-authors")]
        public async Task<IActionResult> GetToursByAuthors([FromBody] List<string> authorIds)
        {
            try
            {
                if (authorIds == null || !authorIds.Any())
                {
                    return Ok(new List<TourDto>());
                }

                var allTours = new List<TourDto>();

                foreach (var authorId in authorIds)
                {
                    var tours = await _tourService.GetToursByAuthorAsync(authorId);
                    allTours.AddRange(tours);
                }

                return Ok(allTours);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "authorPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTour(int id, [FromBody] TourDto tourDto)
        {
            try
            {
                var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(authorId)) return Unauthorized();

                tourDto.AuthorId = authorId;
                var updatedTour = await _tourService.UpdateTourAsync(id, tourDto);
                return Ok(updatedTour);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize(Policy = "authorPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTour(int id)
        {
            try
            {
                await _tourService.DeleteTourAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "authorPolicy")]
        [HttpPut("tour-metrics/{id}")]
        public async Task<IActionResult> UpdateTourMetrics(int id, [FromBody] TourMetricsDto tourMetrics)
        {
            try
            {
                var updatedTour = await _tourService.UpdateTourMetrics(id, tourMetrics);
                return Ok(updatedTour);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize(Policy = "authorPolicy")]
        [HttpPut("tour-status/{id}")]
        public async Task<IActionResult> UpdateTourStatus(int id)
        {
            try
            {
                var updatedTour = await _tourService.UpdateTourStatus(id);
                return Ok(updatedTour);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
