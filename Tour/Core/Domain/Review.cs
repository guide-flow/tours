using System;
using API.Dtos;
using Common.Enums;

namespace Core.Domain
{
    public class Review
    {
        public int Id { get; set; }
        public Rating Rating { get; set; }
        public string Comment { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime VisitedAt { get; set; }
        public string TouristId { get; set; } = default!;
        public string? ImageUrl { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; } = default!;

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
