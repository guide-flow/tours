using Core.Domain;
using Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly ToursContext _context;
        public TagRepository(ToursContext context)
        {
            _context = context;
        }
        public async Task<Tag?> GetByNameAsync(string name)
        {
            return await _context.Tags
                .FirstOrDefaultAsync(t => t.Name == name) ?? null;
        }
    }
}
