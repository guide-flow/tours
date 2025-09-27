using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.RepositoryInterfaces
{
    public interface ITourRepository
    {
        Task<Tour> GetByIdAsync(int id);
        Task<IEnumerable<Tour>> GetByAuthorAsync(string authorId);
        Task AddAsync(Tour tour);
        Task UpdateAsync(Tour tour);

    }
}
