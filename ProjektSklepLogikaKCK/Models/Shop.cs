using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSklepLogikaKCK.Models
{
    public class Shop
    {

        public string shopName = "AllegroV2";

        public List<Category> categories = new List<Category>();

        public List<Product> products = new List<Product>();

        public void InitCategories()
        {
            try
            {
                foreach (var line in File.ReadAllLines("kategorie.txt"))
                {
                    var parts = line.Split(' ');

                    if (parts.Length == 2 && int.TryParse(parts[1], out int vat))
                    {
                        string categoryName = parts[0];
                        categories.Add(new Category(categoryName, vat));
                    }
                    else
                    {
                        Console.WriteLine($"Invalid line format: {line}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
        }

        public void InitProducts()
        {
            try
            {
                foreach (var line in File.ReadAllLines("produkty.txt"))
                {
                    var parts = line.Split(' ');

                    if (parts.Length >= 4 && float.TryParse(parts[2], out float price))
                    {
                        string productName = parts[0];
                        string description = parts[1].Replace("_", " "); // Zamienia podkreślenia na spacje
                        string categoryName = parts[3];

                        // Znajdź kategorię na podstawie nazwy
                        Category category = categories.FirstOrDefault(c => c.name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

                        if (category != null)
                        {
                            products.Add(new Product(productName, description, price, category));
                        }
                        else
                        {
                            Console.WriteLine($"Category '{categoryName}' not found for product '{productName}'.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Invalid line format in products file: {line}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading products file: {ex.Message}");
            }
        }

        public List<Product> FilterProductsByName(string partialName)
        {
            return products.Where(p => p.name.StartsWith(partialName, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Product> FilterProductsByCategory(string categoryName)
        {
            return products.Where(p => p.category.name.StartsWith(categoryName, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public void StartShop()
        {
            InitCategories();
            InitProducts();
        }


    }
}
