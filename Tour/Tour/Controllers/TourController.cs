using API.Dtos;
using API.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Tour.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TourController : ControllerBase
    {
        private readonly ITourService _tourService;

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTour([FromBody] TourDto tourDto)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(authorId))
            {
                return Unauthorized();
            }
            tourDto.AuthorId = authorId;   
            var createdTour = await _tourService.CreateTourAsync(tourDto);
            return Ok(createdTour);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTourById(int id)
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            if (tour == null)
            {
                return NotFound();
            }
            return Ok(tour);
        }
        [Authorize]
        [HttpGet("author")]
        public async Task<IActionResult> GetToursByAuthor()
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(authorId))
            {
                return Unauthorized();
            }
            var tours = await _tourService.GetToursByAuthorAsync(authorId);
            return Ok(tours);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTour(int id, [FromBody] TourDto tourDto)
        {
            try
            {
                var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(authorId))
                {
                    return Unauthorized();
                }
                tourDto.AuthorId = authorId;
                var updatedTour = await _tourService.UpdateTourAsync(id, tourDto);
                return Ok(updatedTour);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
