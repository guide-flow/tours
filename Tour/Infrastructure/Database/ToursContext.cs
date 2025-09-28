using Core.Domain;
using Core.Domain.Shopping;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public class ToursContext : DbContext
    {
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Checkpoint> Checkpoints { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
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
        }
    }
}
