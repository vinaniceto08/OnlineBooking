using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using TravelTayo.Models;

namespace TravelTayo.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Hotel> Hotels { get; set; }
    }
}
