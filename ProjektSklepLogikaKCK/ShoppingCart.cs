using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSklepLogikaKCK
{
    public class ShoppingCart
    {
        private List<Product> productsInCart = new List<Product>();
        private float fullprice;

        public void AddItem(Product product)
        {
            productsInCart.Add(product);
            fullprice += product.price;
        }

        public void DeleteItem(Product product) 
        {
            productsInCart.Remove(product);
            fullprice -= product.price;
        }

        public List<Product> GetProducts()
        {
            return productsInCart;
        }

        public float GetFullprice() {  return fullprice; }



        public void EmptyCart()
        {
            foreach (Product product in productsInCart)
            {
                productsInCart.Remove(product); fullprice -= product.price;
            }
        }


    }
}
