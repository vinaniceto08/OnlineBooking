using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using TravelTayo.Models;

namespace TravelTayo.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>()
                .Property(h => h.Price)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }

    }


}
