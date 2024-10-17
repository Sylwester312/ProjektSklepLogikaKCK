using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSklepLogikaKCK;
class Program
{
    static void Main(string[] args)
    {
        Shop shop = new Shop();

        shop.StartShop();

         //foreach (Product product in shop.products) 
         //{
         //    Console.WriteLine(product.ToStringNetto());
         //}

        /*var filteredProducts = shop.FilterProductsByName("L");

        // Wyświetlamy wyniki filtrowania
        foreach (var product in filteredProducts)
        {
            Console.WriteLine(product.ToString());
        }*/

        /*Client client = new Client("Jan", "Jeleń", "example@mail.com");

        client.cart.AddItem(shop.products[0]);
        client.cart.AddItem(shop.products[3]);
        client.cart.AddItem(shop.products[5]);
        client.cart.AddItem(shop.products[8]);

        foreach (var item in client.cart.GetProducts())
        {
            Console.WriteLine(item.ToString());
        }
        Console.WriteLine(client.cart.GetFullprice());

        client.cart.DeleteItem(shop.products[5]);

        foreach (var item in client.cart.GetProducts())
        {
            Console.WriteLine(item.ToString());
        }
        Console.WriteLine(client.cart.GetFullprice());*/


        /*var filteredProductsByCategory = shop.FilterProductsByCategory("Jadło");

        foreach (var product in filteredProductsByCategory)
        {
            Console.WriteLine(product.ToString());
        }*/

        


    }
}
