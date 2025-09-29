using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Dtos;
using API.ServiceInterfaces;
using AutoMapper;
using Common.Enums;
using Core.Domain;
using Core.Domain.RepositoryInterfaces;

namespace Core.UseCases
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repository;

        public ReviewService(IReviewRepository repository)
        {
            _repository = repository;
        }

        public async Task<ReviewDto> AddReviewAsync(string touristId, int tourId, Rating rating, string comment, DateTime visitedAt, string? imageUrl)
        {
            var review = ReviewDto.Create(touristId, tourId, rating, comment, visitedAt, imageUrl);
            return await _repository.AddAsync(review);
        }
    }

}
