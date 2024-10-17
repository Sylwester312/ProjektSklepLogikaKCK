using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSklepLogikaKCK
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

        public override string ToString() 
        {
            return name + " | " + description + " | " + price + " | " + category.ToString();
        }

        public string ToStringNetto()
        {
            return name + " | " + description + " | " + (price*=((float)(100-category.vat)/100)) + " | " + category.ToString();
        }



        public string ProductCategoryToString() { return category.ToString(); }

    }
}
