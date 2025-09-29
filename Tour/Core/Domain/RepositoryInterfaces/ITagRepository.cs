using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.RepositoryInterfaces
{
    public interface ITagRepository
    {
        Task<Tag?> GetByNameAsync(string name);
    }
}
