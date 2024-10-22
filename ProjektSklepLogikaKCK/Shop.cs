using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSklepLogikaKCK
{
    public class Shop
    {
        
        public string shopName = "AllegroV2";
        
        public List<Category> categories = new List<Category>();

        public List<Product> products = new List<Product>();

        public void InitCategories()
        {
            categories.Add(new Category("Elektronika", 23));
            categories.Add(new Category("Moda", 8));
            categories.Add(new Category("Dom", 5));
            categories.Add(new Category("Jadło", 5));
        }

        public void InitProducts()
        {
            // Produkty dla kategorii 'Electronics'
            products.Add(new Product("Smartphone", "Nowoczesny telefon z dużym ekranem", (float)1200.0, categories[0]));
            products.Add(new Product("Laptop", "Laptop z procesorem i7", (float)3500.0, categories[0]));
            products.Add(new Product("Telewizor", "Telewizor 55 cali 4K", (float)2200.0, categories[0]));
            products.Add(new Product("Słuchawki", "Bezprzewodowe słuchawki z redukcją szumów", (float)500.0, categories[0]));
            products.Add(new Product("Smartwatch", "Zegarek sportowy z GPS", (float)800.0, categories[0]));

            // Produkty dla kategorii 'Fashion'
            products.Add(new Product("Kurtka", "Ciepła zimowa kurtka", (float)300.0, categories[1]));
            products.Add(new Product("Buty", "Stylowe buty sportowe", (float)250.0, categories[1]));
            products.Add(new Product("Sukienka", "Elegancka sukienka wieczorowa", (float)450.0, categories[1]));
            products.Add(new Product("T-shirt", "Bawełniany t-shirt", (float)50.0, categories[1]));
            products.Add(new Product("Jeansy", "Wygodne jeansy slim fit", (float)200.0, categories[1]));

            // Produkty dla kategorii 'Home'
            products.Add(new Product("Sofa", "Wygodna sofa 3-osobowa", (float)1500.0, categories[2]));
            products.Add(new Product("Lampa", "Nowoczesna lampa stojąca", (float)200.0, categories[2]));
            products.Add(new Product("Stół", "Drewniany stół do jadalni", (float)1200.0, categories[2]));
            products.Add(new Product("Krzesła", "Zestaw 4 krzeseł", (float)600.0, categories[2]));
            products.Add(new Product("Zasłony", "Eleganckie zasłony do salonu", (float)300.0, categories[2]));

            // Produkty dla kategorii 'Food'
            products.Add(new Product("Jabłko", "Smaczne czerwone", (float)3.0, categories[3]));
            products.Add(new Product("Chleb", "Świeży chleb pszenny", (float)4.0, categories[3]));
            products.Add(new Product("Mleko", "Mleko 2% 1L", (float)2.5, categories[3]));
            products.Add(new Product("Ser", "Ser żółty 250g", (float)10.0, categories[3]));
            products.Add(new Product("Makaron", "Makaron penne 500g", (float)6.0, categories[3]));
        }

        public List<Product> FilterProductsByName(string partialName)
        {
            // Zwracamy produkty, których nazwa zaczyna się od podanego fragmentu (bez rozróżniania wielkości liter)
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
