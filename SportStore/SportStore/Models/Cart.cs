using System.Collections.Generic;
using System.Linq;

namespace SportStore.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public virtual void AddItem(Product product, int quantity)
        {
            var line = lineCollection
                .FirstOrDefault(x => x.Product.ProductId == product.ProductId);

            if (line == null)
            {
                lineCollection.Add(new CartLine() { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product) => lineCollection.RemoveAll(x => x.Product.ProductId == product.ProductId);

        public virtual decimal ComputeTotalValue() => lineCollection.Sum(x => x.Product.Price * x.Quantity);

        public virtual void Clear() => lineCollection.Clear();

        public virtual IEnumerable<CartLine> Lines => lineCollection;
    }

    public class CartLine
    {
        public int CartLineID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}