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

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTour([FromBody] TourDto tourDto)
        {
            //var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (string.IsNullOrEmpty(authorId))
            //{
            //    return Unauthorized();
            //}
            tourDto.AuthorId = "1";   
            var createdTour = await _tourService.CreateTourAsync(tourDto,"1");
            return Ok(createdTour);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTourById(int id)
        {
            try
            {
                var tour = await _tourService.GetTourByIdAsync(id);
                if (tour == null)
                {
                    return NotFound();
                }
                return Ok(tour);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[Authorize]
        [HttpGet("author")]
        public async Task<IActionResult> GetToursByAuthor()
        {
            try
            {
                //var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //if (string.IsNullOrEmpty(authorId))
                //{
                //    return Unauthorized();
                //}
                var tours = await _tourService.GetToursByAuthorAsync("1");
                return Ok(tours);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTour(int id, [FromBody] TourDto tourDto)
        {
            try
            {
                //var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //if (string.IsNullOrEmpty(authorId))
                //{
                //    return Unauthorized();
                //}
                tourDto.AuthorId = "1";
                var updatedTour = await _tourService.UpdateTourAsync(id, tourDto);
                return Ok(updatedTour);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTour(int id)
        {
            try
            {
                await _tourService.DeleteTourAsync(id);
                return Ok();
            }catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        //[Authorize]
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

        //[Authorize]
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
