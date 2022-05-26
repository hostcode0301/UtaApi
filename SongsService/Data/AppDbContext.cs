using Microsoft.EntityFrameworkCore;
using SongsService.Models;

namespace SongsService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions opts) : base(opts)
        {
        }

        public DbSet<Song> Songs { get; set; } = null!;
    }
}