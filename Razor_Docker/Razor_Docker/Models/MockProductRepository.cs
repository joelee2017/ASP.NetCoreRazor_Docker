using System.Linq;

namespace Razor_Docker.Models
{
    public class MockProductRepository : IProductRepository
    {

        private static readonly Product[] DummyData = new Product[] {
            new Product { Name = "產品1", Category = "分類1", Price = 100 },
            new Product { Name = "產品2", Category = "分類1", Price = 100 },
            new Product { Name = "產品3", Category = "分類2", Price = 100 },
            new Product { Name = "產品4", Category = "分類2", Price = 100 },
            };
        public IQueryable<Product> Products => DummyData.AsQueryable();

    }
}
