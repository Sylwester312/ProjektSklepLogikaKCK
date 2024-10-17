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
        private float fullpriceNetto;

        public void AddItem(Product product)
        {
            productsInCart.Add(product);
            fullprice += product.price;
            fullpriceNetto += product.price *= ((float)(100 - product.category.vat) / 100);
        }

        public void DeleteItem(Product product)
        {
            productsInCart.Remove(product);
            fullprice -= product.price;
            fullpriceNetto -= product.price *= ((float)(100 - product.category.vat) / 100);
        }

        public List<Product> GetProducts()
        {
            return productsInCart;
        }

        public float GetFullprice() { return fullprice; }

        public float GetFullpriceNetto() { return fullpriceNetto; }



        public void EmptyCart()
        {
            foreach (Product product in productsInCart)
            {
                productsInCart.Remove(product); fullprice -= product.price; fullpriceNetto -= product.price *= ((float)(100 - product.category.vat) / 100);
            }
        }


    }
}