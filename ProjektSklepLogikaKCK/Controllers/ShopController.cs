using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjektSklepLogikaKCK.DTOs;
using ProjektSklepLogikaKCK.Models;

namespace ProjektSklepLogikaKCK.Controllers
{
    public class ShopController
    {
        public Shop shop;

        public ShopController()
        {
            shop = new Shop();
            shop.StartShop();
        }
        public List<Product> GetAllProducts()
        {
            return shop.products;
        }

        public void AddProduct(Product product)
        {
            shop.products.Add(product);
        }

        public void DeleteProduct(Product product) 
        { 
            shop.products.Remove(product);
        }

        public List<Product> FilterProductsByName(string name)
        {
            return shop.FilterProductsByName(name);
        }

        public List<Product> FilterProductsByCategory(string categoryName)
        {
            return shop.FilterProductsByCategory(categoryName);
        }

        public List<ProductDTO> FilterProductsByNameDTO(string name)
        {
            var list = shop.FilterProductsByName(name);
            return list.Select(p => new ProductDTO
            {
                Name = p.name,
                Price = p.price,
                Description = p.description,
                Category = p.category
            }).ToList();
        }

        public List<ProductDTO> FilterProductsByCategoryDTO(string categoryName)
        {
            var list = shop.FilterProductsByCategory(categoryName);
            return list.Select(p => new ProductDTO
            {
                Name = p.name,
                Price = p.price,
                Description = p.description,
                Category = p.category
            }).ToList();
        }

        public string GetShopName()
        {
            return shop.shopName;
        }

        public Product CreateProduct(string name, string description, float price, Category category)
        {
            return new Product(name, description, price, category);
        }

        public Category CreateCategory(string name, int vat)
        { 
            return new Category(name, vat);
        }

        public List<ProductDTO> GetProducts()
        {
            return shop.products.Select(p => new ProductDTO
            {
                Name = p.name,
                Price = p.price,
                Description = p.description,
                Category = p.category
            }).ToList();
        }


    }
}
