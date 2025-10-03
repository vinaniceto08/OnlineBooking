
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using TravelTayo.Models;
using TravelTayo.Pages;

namespace TravelTayo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    
        public DbSet<ReferralRegistration> ReferralRegistrations {get; set;}
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryGroup> CategoryGroups { get; set; }
        public DbSet<Chain> Chains { get; set; }
        public DbSet<AccommodationType> AccommodationTypes { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Terminal> Terminals { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<HotelPhone> HotelPhones { get; set; }
        public DbSet<HotelWildcard> HotelWildcards { get; set; }
        //public DbSet<Room> Rooms { get; set; }
        //public DbSet<Facility> Facilities { get; set; }
        //public DbSet<Terminal> Terminals { get; set; }
        //public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReferralRegistration>(entity =>
            {
                modelBuilder.Entity<Hotel>()
                 .HasOne(h => h.Country)
                 .WithMany(c => c.Hotels)
                 .HasForeignKey(h => h.CountryId)
                 .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<Hotel>()
                    .HasOne(h => h.State)
                    .WithMany(s => s.Hotels)
                    .HasForeignKey(h => h.StateId)
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<Hotel>()
                    .HasOne(h => h.Destination)
                    .WithMany(d => d.Hotels)
                    .HasForeignKey(h => h.DestinationId)
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<Hotel>()
                    .HasOne(h => h.Zone)
                    .WithMany(z => z.Hotels)
                    .HasForeignKey(h => h.ZoneId)
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<Hotel>()
                    .HasOne(h => h.Category)
                    .WithMany(c => c.Hotels)
                    .HasForeignKey(h => h.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<Hotel>()
                    .HasOne(h => h.CategoryGroup)
                    .WithMany(cg => cg.Hotels)
                    .HasForeignKey(h => h.CategoryGroupId)
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<Hotel>()
                    .HasOne(h => h.Chain)
                    .WithMany(c => c.Hotels)
                    .HasForeignKey(h => h.ChainId)
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<Hotel>()
                    .HasOne(h => h.AccommodationType)
                    .WithMany(a => a.Hotels)
                    .HasForeignKey(h => h.AccommodationTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<Hotel>()
                    .HasOne(h => h.Address)
                    .WithMany(a => a.Hotels)
                    .HasForeignKey(h => h.AddressId)
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<HotelPhone>()
                    .HasOne(p => p.Hotel)
                    .WithMany(h => h.Phones)
                    .HasForeignKey(p => p.HotelId);

                modelBuilder.Entity<HotelWildcard>()
                    .HasOne(w => w.Hotel)
                    .WithMany(h => h.Wildcards)
                    .HasForeignKey(w => w.HotelId);

                entity.HasKey(e => e.ReferralRegistrationId);

                entity.HasIndex(e => e.EmailAddress)
                      .IsUnique(); // Very important for fast lookups

                entity.Property(e => e.EmailAddress)
                      .IsRequired()
                      .HasMaxLength(256);

                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.GCashQRCode).HasMaxLength(500);

                modelBuilder.Entity<Account>()
               .HasIndex(a => a.B2CUserId)
               .IsUnique();
            });
        }


    }


}
