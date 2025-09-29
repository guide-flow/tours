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
    public class CheckpointRepository : ICheckpointRepository
    {
        private readonly ToursContext _context;

        public CheckpointRepository(ToursContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Checkpoint checkpoint)
        {
            await _context.Checkpoints.AddAsync(checkpoint);
            await _context.SaveChangesAsync();
        }
        public async Task<Checkpoint> GetByIdAsync(int id)
        {
            return await _context.Checkpoints.FindAsync(id) ?? throw new KeyNotFoundException($"Checkpoint with id {id} not found.");
        }
        public async Task DeleteAsync(int id)
        {
            var checkpoint = await _context.Checkpoints.FindAsync(id);
            if (checkpoint == null)
            {
                throw new KeyNotFoundException($"Checkpoint with id {id} not found.");
            }
            _context.Checkpoints.Remove(checkpoint);
            await _context.SaveChangesAsync();
        }

        public async Task<Checkpoint> UpdateAsync(Checkpoint checkpoint) {
            var existing = await _context.Checkpoints.FindAsync(checkpoint.Id);
            if (existing == null)
                throw new Exception("Checkpoint not found");

            existing.Title = checkpoint.Title;
            existing.Description = checkpoint.Description;
            existing.Latitude = checkpoint.Latitude;
            existing.Longitude = checkpoint.Longitude;
            existing.ImageUrl = checkpoint.ImageUrl;

            await _context.SaveChangesAsync();
            return existing;
        }
    }
}
