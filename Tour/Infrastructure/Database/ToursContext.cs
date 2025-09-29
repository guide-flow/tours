using Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database
{
    public class ToursContext : DbContext
    {
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Checkpoint> Checkpoints { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public ToursContext(DbContextOptions<ToursContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("tours");

            modelBuilder.Entity<Tour>()
                .HasMany(t => t.Checkpoints)
                .WithOne(c => c.Tour)
                .HasForeignKey(c => c.TourId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Tour>()
                .HasMany(t => t.Reviews)
                .WithOne(r => r.Tour)
                .HasForeignKey(r => r.TourId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Tour>()
                .HasMany(t => t.Tags)
                .WithMany(tag => tag.Tours)
                .UsingEntity(j => j.ToTable("TourTags"));

            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(r => r.Comment)
                      .IsRequired()
                      .HasMaxLength(1000);

                entity.Property(r => r.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(r => r.Rating)
                      .IsRequired();
            });
        }
    }
}
