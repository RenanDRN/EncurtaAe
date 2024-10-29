using Microsoft.EntityFrameworkCore;

namespace App.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UrlModel> Urls { get; set; }
    }
}