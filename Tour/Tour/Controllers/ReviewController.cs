using System.Security.Claims;
using API.Dtos;
using API.ServiceInterfaces;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tour.Controllers
{
    [ApiController]
    [Route("api/tours/{tourId}/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int tourId, [FromBody] CreateReviewRequest request)
        {
            var touristId = User.Identity?.Name ?? "demo-user"; 

            var review = await _reviewService.AddReviewAsync(
                touristId,
                tourId,
                (Rating)request.Rating,
                request.Comment,
                request.VisitedAt,
                request.ImageUrl
            );

            return Ok(review);
        }
    }

    public class CreateReviewRequest
    {
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime VisitedAt { get; set; }
        public string? ImageUrl { get; set; }
    }

}
