using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Dtos;
using Common.Enums;

namespace API.ServiceInterfaces
{
    public interface IReviewService
    {
        Task<ReviewDto> AddReviewAsync(string touristId, int tourId, Rating rating, string comment, DateTime visitedAt, string? imageUrl);
    }

}
