using Spectre.Console;
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

        Client client = new Client("Jan", "Jeleń", "example@mail.com");


        /*
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

        // Wyświetlenie strony tytułowej
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Welcome to the Shop!")
                .Centered()
                .Color(Color.Green));

        // Dodanie przycisku "Dalej"
        AnsiConsole.MarkupLine("\n[bold]Press [green]Enter[/] to continue...[/]");
        Console.ReadLine(); // Czekanie na naciśnięcie klawisza Enter

        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose an option:")
                    .AddChoices("Koszyk", "Wyszukiwarka", "Lista Produktów", "Exit"));

            switch (choice)
            {
                case "Koszyk":
                    ShowCart(client.cart.GetProducts());
                    break;

                case "Wyszukiwarka":
                    SearchProduct(shop.products);
                    break;

                case "Lista Produktów":
                    ShowProductList(shop.products, client.cart.GetProducts());
                    break;

                case "Exit":
                    return;
            }
        }

        static void ShowCart(List<Product> cart)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold yellow]Your cart contains the following items:[/]");
            if (cart.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]The cart is empty.[/]");
                return;
            }

            foreach (var item in cart)
            {
                AnsiConsole.MarkupLine($"[green]{item.name}[/] - {item.price:C}");
            }

            // Obliczanie sumy
            float total = 0;
            foreach (var item in cart)
            {
                total += item.price;
            }

            AnsiConsole.MarkupLine($"\n[bold]Total price:[/] [blue]{total:C}[/]");
        }

        static void SearchProduct(List<Product> products)
        {
            AnsiConsole.Clear();
            string searchTerm = AnsiConsole.Ask<string>("Enter the [green]product name[/] to search:");

            var foundProducts = products.FindAll(p => p.name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

            if (foundProducts.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No products found.[/]");
                return;
            }

            AnsiConsole.MarkupLine("[bold yellow]Search Results:[/]");
            foreach (var product in foundProducts)
            {
                AnsiConsole.MarkupLine($"[green]{product.name}[/] - {product.price:C}");
            }
        }

        static void ShowProductList(List<Product> products, List<Product> cart)
        {
            AnsiConsole.Clear();
            while (true)
            {
                var selectedProduct = AnsiConsole.Prompt(
                    new SelectionPrompt<Product>()
                        .Title("Select a [green]product[/] to add to the cart or [red]press ESC to go back[/]:")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more products)[/]")
                        .AddChoices(products)
                        .UseConverter(p => $"{p.name} - {p.price:C}")
                );

                // Dodanie wybranego produktu do koszyka
                cart.Add(selectedProduct);
                AnsiConsole.MarkupLine($"[green]Added to cart:[/] {selectedProduct.name}");

                // Pytanie, czy użytkownik chce dodać kolejny produkt
                bool addMore = AnsiConsole.Confirm("Do you want to add another product to the cart?");
                if (!addMore)
                {
                    break;
                }
            }
        }
    

    


    }
}
