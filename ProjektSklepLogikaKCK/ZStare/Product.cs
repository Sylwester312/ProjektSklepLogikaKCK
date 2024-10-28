using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSklepLogikaKCK.ZStare
{
    public class Product
    {
        public string name;
        public string description;
        public float price;
        public Category category;

        public Product(string name, string description, float price, Category category)
        {
            this.name = name;
            this.description = description;
            this.price = price;
            this.category = category;
        }

    }
}
