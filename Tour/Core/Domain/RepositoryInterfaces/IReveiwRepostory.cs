using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Dtos;

namespace Core.Domain.RepositoryInterfaces
{
    public interface IReviewRepository
    {
        Task<Review> AddAsync(Review review);
        Task<ReviewDto> AddAsync(ReviewDto review);
    }
}
