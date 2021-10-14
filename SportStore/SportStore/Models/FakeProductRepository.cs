using System.Collections.Generic;
using System.Linq;

namespace SportStore.Models
{
    public class FakeProductRepository : IProductRepository
    {
        public IQueryable<Product> Products => new List<Product>
        {
            new Product(){Name = "Football", Price = 25},
            new Product(){Name = "Stuff Board", Price = 179},
            new Product(){Name = "Running Shoes", Price = 95},
        }.AsQueryable();
    }
}