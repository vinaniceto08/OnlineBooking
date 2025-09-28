using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using TravelTayo.Models;

namespace TravelTayo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<ReferralRegistration> ReferralRegistrations {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReferralRegistration>(entity =>
            {
                modelBuilder.Entity<Hotel>()
                        .Property(h => h.Price)
                        .HasPrecision(18, 2);

                entity.HasKey(e => e.ReferralRegistrationId);

                entity.HasIndex(e => e.EmailAddress)
                      .IsUnique(); // Very important for fast lookups

                entity.Property(e => e.EmailAddress)
                      .IsRequired()
                      .HasMaxLength(256);

                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.GCashNumber).HasMaxLength(20);
                entity.Property(e => e.GCashQRCode).HasMaxLength(500);
            });
        }


    }


}
