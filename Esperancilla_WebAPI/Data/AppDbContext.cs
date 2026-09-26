using Microsoft.EntityFrameworkCore;
using Esperancilla_WebAPI.Model;

namespace Esperancilla_WebAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
