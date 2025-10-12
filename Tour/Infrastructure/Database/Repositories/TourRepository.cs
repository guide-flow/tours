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
    public class TourRepository : ITourRepository
    {
        private readonly ToursContext _context;
        public TourRepository(ToursContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Tour tour)
        {
            await _context.Tours.AddAsync(tour);
            await _context.SaveChangesAsync();
        }

        public async Task<Tour> GetByIdAsync(int id)
        {
            return await _context.Tours.FirstOrDefaultAsync(t => t.Id == id) ?? throw new KeyNotFoundException($"Tour with id {id} not found.");
        }

        public async Task<Tour> GetByIdWithCheckpoints(int id) { 
            return await _context.Tours.Include(t => t.Checkpoints).Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id == id) ?? throw new KeyNotFoundException($"Tour with id {id} not found.");
        }

        public async Task<IEnumerable<Tour>> GetByAuthorAsync(string authorId)
        {
            return await _context.Tours.Include(t => t.Tags).Where(t => t.AuthorId == authorId).ToListAsync();
        }

        public async Task UpdateAsync(Tour tour)
        {
            _context.Tours.Update(tour);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                throw new KeyNotFoundException($"Tour with id {id} not found.");
            }
            _context.Tours.Remove(tour);
            await _context.SaveChangesAsync();
        }
    }
}
