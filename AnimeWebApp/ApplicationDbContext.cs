using AnimeWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeWebApp
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> PoProfiles { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}