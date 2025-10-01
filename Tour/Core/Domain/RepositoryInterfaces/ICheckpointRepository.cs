using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.RepositoryInterfaces
{
    public interface ICheckpointRepository
    {
        Task AddAsync(Checkpoint checkpoint);
        Task<Checkpoint> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<Checkpoint> UpdateAsync(Checkpoint checkpoint);
    }
}
