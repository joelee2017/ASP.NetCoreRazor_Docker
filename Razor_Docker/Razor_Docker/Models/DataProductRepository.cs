using System.Linq;

namespace Razor_Docker.Models
{
    public class DataProductRepository : IProductRepository
    {
        private ProductDbContext context;

        public DataProductRepository(ProductDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Product> Products => context.Products;
    }
}
