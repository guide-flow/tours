using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public Rating Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime VisitedAt { get; set; }
        public string TouristId { get; set; }
        public string? ImageUrl { get; set; }
        public int TourId { get; set; }
        public TourDto Tour { get; set; }
    public static ReviewDto Create(
            string touristId,
            int tourId,
            Rating rating,
            string comment,
            DateTime visitedAt,
            string? imageUrl = null)
        {
            return new ReviewDto
            {
                TouristId = touristId,
                TourId = tourId,
                Rating = rating,
                Comment = comment,
                VisitedAt = visitedAt,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
