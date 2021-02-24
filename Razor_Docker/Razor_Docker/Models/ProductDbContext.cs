using Microsoft.EntityFrameworkCore;

namespace Razor_Docker.Models
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
        : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
    }
}
