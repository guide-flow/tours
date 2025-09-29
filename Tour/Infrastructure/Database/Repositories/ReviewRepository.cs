using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Dtos;
using Common.Enums;
using Core.Domain;
using Core.Domain.RepositoryInterfaces;

namespace Infrastructure.Database.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ToursContext _context;

        public ReviewRepository(ToursContext context)
        {
            _context = context;
        }

        public async Task<Review> AddAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<ReviewDto> AddAsync(ReviewDto reviewDto)
        {
            // Map ReviewDto to Review entity
            var review = new Review
            {
                TouristId = reviewDto.TouristId,
                TourId = reviewDto.TourId,
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment,
                VisitedAt = reviewDto.VisitedAt,
                ImageUrl = reviewDto.ImageUrl,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return reviewDto;
        }
    }
}
