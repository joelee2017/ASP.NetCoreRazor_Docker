using System.Linq;

namespace Razor_Docker.Models
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }

    }
}
