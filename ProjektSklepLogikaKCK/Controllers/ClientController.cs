using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjektSklepLogikaKCK.DTOs;
using ProjektSklepLogikaKCK.Models;


namespace ProjektSklepLogikaKCK.Controllers
{
    public class ClientController
    {
        public Client client;
        public ClientController(string name, string surname, string mail)
        {
            client = new Client(name, surname, mail);
        }

        public void AddProductToClientCart(Product product)
        {
            client.cart.AddItem(product);
        }
        public void AddProductToClientCartDTO(ProductDTO product)
        {
            var newProd = new Product(
            product.Name,
            product.Description,
            product.Price,
            product.Category
        );
            client.cart.AddItem(newProd);
        }

        public void DeleteProductFromClientCart(Product product)
        {
            client.cart.DeleteItem(product);
        }

        public void DeleteProductFromClientCartDTO(ProductDTO product)
        {
            var newProd = new Product(
            product.Name,
            product.Description,
            product.Price,
            product.Category
        );

            client.cart.DeleteItem(newProd);
        }

        public void ClearCart()
        {
            client.cart.EmptyCart();
        }

        public List<ProductDTO> GetProducts()
        {
            return client.cart.productsInCart.Select(p => new ProductDTO
            {
                Name = p.name,
                Price = p.price,
                Description = p.description,
                Category = p.category
            }).ToList();
        }

        public int CountProductsInCart()
        {
            return client.cart.productsInCart.Count;
        }

        public bool WantNetto()
        {
            return client.wantNetto;
        }

        public void ChangeVAT(bool value)
        {
            client.wantNetto = value;
        }

        public float GetFullCartPrice()
        {
            return client.cart.GetFullprice();
        }

        public float GetFullCartPriceNetto()
        {
            return client.cart.GetFullpriceNetto();
        }
    }
}
