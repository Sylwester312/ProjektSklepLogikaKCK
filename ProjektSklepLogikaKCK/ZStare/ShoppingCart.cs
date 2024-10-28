using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSklepLogikaKCK.ZStare
{
    public class ShoppingCart
    {
        public List<Product> productsInCart = new List<Product>();
        public float fullprice;
        public float fullpriceNetto;

        public void AddItem(Product product)
        {
            productsInCart.Add(product);
            fullprice += product.price;
            fullpriceNetto += product.price * ((float)(100 - product.category.vat) / 100);
        }

        public void DeleteItem(Product product)
        {
            productsInCart.Remove(product);
            fullprice -= product.price;
            fullpriceNetto -= product.price * ((float)(100 - product.category.vat) / 100);
        }

        public List<Product> GetProducts()
        {
            return productsInCart;
        }

        public float GetFullprice() { return fullprice; }

        public float GetFullpriceNetto() { return fullpriceNetto; }



        public void EmptyCart()
        {
            productsInCart = new List<Product>();
            fullprice = 0;
            fullpriceNetto = 0;
        }


    }
}