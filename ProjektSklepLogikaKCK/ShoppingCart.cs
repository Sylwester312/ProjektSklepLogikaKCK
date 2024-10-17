using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSklepLogikaKCK
{
    public class ShoppingCart
    {
        private List<Product> products;
        private float fullprice;

        public void AddItem(Product product)
        {
            products.Add(product);
            fullprice += product.price;
        }

        public void DeleteItem(Product product) 
        {
            products.Remove(product);
            fullprice -= product.price;
        }

        public List<Product> GetProducts()
        {
            return products;
        }

        public float GetFullprice() {  return fullprice; }

 
        public void EmptyCart()
        {
            foreach (Product product in products)
            {
                products.Remove(product); fullprice -= product.price;
            }
        }

    }
}
