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

        // Pobieranie danych użytkownika (imię, nazwisko, e-mail)
        string firstName = AnsiConsole.Ask<string>("Enter your [green]First Name[/]:");
        string lastName = AnsiConsole.Ask<string>("Enter your [green]Last Name[/]:");
        string email = AnsiConsole.Ask<string>("Enter your [green]Email[/]:");

        Client client = new Client(firstName, lastName, email);

        AnsiConsole.MarkupLine($"\n[bold]Client created:[/] {client.name} {client.surname} - [blue]{client.mail}[/]\n");

        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose an option:")
                    .AddChoices("Koszyk", "Wyszukiwarka", "Lista Produktów", "Czy chcesz ceny netto", "Exit"));

            switch (choice)
            {
                case "Koszyk":
                    ShowCart(client);
                    break;

                case "Wyszukiwarka":
                    SearchProduct(client, shop);
                    break;

                case "Lista Produktów":
                    ShowProductList(shop.products, client);
                    break;
                case "Czy chcesz ceny netto":
                    ToggleNetPrices(client);
                    break;

                case "Exit":
                    return;
            }
        }

        static void ShowCart(Client client)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold yellow]Your cart contains the following items:[/]");
            if (client.cart.productsInCart.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]The cart is empty.[/]");
                return;
            }

            // Dodanie opcji "Powrót" do listy wyników
            var sysCat = new Category("Systemowe", 0);
            var powrot = new Product("Powrót", "Wróć do poprzedniego menu", 0f, sysCat);
            var zamow = new Product("Zamów", "Finalizacja zamówienia", 0f, sysCat);

            client.cart.AddItem(powrot);
            client.cart.AddItem(zamow);

            AnsiConsole.Clear();
            while (true)
            {
                var selectedProduct = AnsiConsole.Prompt(
                    new SelectionPrompt<Product>()
                        .Title("Select a [green]product[/] to add to the cart or [red]press ESC to go back[/]:")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more products)[/]")
                        .AddChoices(client.cart.productsInCart)
                        .UseConverter(p =>
                        {
                            if (p.name == "Powrót" || p.name == "Zamów")
                            {
                                return $"{p.name} ";
                            }
                            else
                            {
                                if (client.wantNetto == false)
                                {
                                    return $"{p.name} - Cena: {p.price:C}";
                                }
                                else
                                {
                                    return $"{p.name} - Cena netto: {(p.price) * ((float)(100 - p.category.vat) / 100):C}";
                                }
                            }

                        })
                );

                if (selectedProduct.name == "Powrót")
                {
                    // Wybrano opcję "Powrót", wychodzimy z pętli
                    break;
                }

                if (selectedProduct.name == "Zamów")
                {
                    client.cart.EmptyCart();
                    AnsiConsole.MarkupLine("[green] Thanks for your order[/]");
                    break;
                }

                client.cart.DeleteItem(selectedProduct);
                AnsiConsole.MarkupLine($"[green]Deleted from cart:[/] {selectedProduct.name}");

                if (client.cart.productsInCart.Count > 2)
                {
                    bool addMore = AnsiConsole.Confirm("Do you want to delete another product from the cart?");
                    if (!addMore)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            client.cart.DeleteItem(powrot);
            client.cart.DeleteItem(zamow);
        }

        static void SearchProduct(Client client, Shop shop)
        {
            AnsiConsole.Clear();

            var searchOption = AnsiConsole.Prompt(
                 new SelectionPrompt<string>()
                 .Title("How would you like to search?")
                 .AddChoices("Name", "Category"));

            var foundProducts = new List<Product>();

            if (searchOption == "Name")
            {
                string searchTerm = AnsiConsole.Ask<string>("Enter the [green]product name[/] to search:");

                foundProducts = shop.FilterProductsByName(searchTerm);

                if (foundProducts.Count == 0)
                {
                    AnsiConsole.MarkupLine("[red]No products found.[/]");
                    return;
                }
            }
            else
            {
                string searchTerm = AnsiConsole.Ask<string>("Enter the [green]product category[/] to search:");

                foundProducts = shop.FilterProductsByCategory(searchTerm);

                if (foundProducts.Count == 0)
                {
                    AnsiConsole.MarkupLine("[red]No products found.[/]");
                    return;
                }
            }



            AnsiConsole.MarkupLine("[bold yellow]Search Results:[/]");
            foreach (var product in foundProducts)
            {
                if(client.wantNetto == false)
                AnsiConsole.MarkupLine($"[green]{product.name}[/] - {product.price:C}");
                else
                    AnsiConsole.MarkupLine($"[green]{product.name}[/] - {(product.price) * ((float)(100 - product.category.vat) / 100):C}");
            }

            // Pytanie, czy użytkownik chce dodać produkty do koszyka
            bool addProduct = AnsiConsole.Confirm("\nDo you want to add any of these products to your cart?");
            if (!addProduct)
            {
                return;
            }

            // Dodawanie produktów do koszyka
            while (true)
            {
                var selectedProduct = AnsiConsole.Prompt(
                    new SelectionPrompt<Product>()
                        .Title("Select a [green]product[/] to add to the cart or [red]press ESC to go back[/]:")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more products)[/]")
                        .AddChoices(foundProducts)
                        .UseConverter(p =>
                        {
                            if (p.name == "Powrót" || p.name == "Zamów")
                            {
                                return $"{p.name} ";
                            }
                            else
                            {
                                if (client.wantNetto == false)
                                {
                                    return $"{p.name} - Cena: {p.price:C}";
                                }
                                else
                                {
                                    return $"{p.name} - Cena netto: {(p.price) * ((float)(100 - p.category.vat) / 100):C}";
                                }
                            }

                        })
                );

                // Dodanie wybranego produktu do koszyka
                client.cart.AddItem(selectedProduct);
                AnsiConsole.MarkupLine($"[green]Added to cart:[/] {selectedProduct.name}");

                break;
            }
        }

        static void ShowProductList(List<Product> products, Client client)
        {
            AnsiConsole.Clear();

            var sysCat = new Category("Systemowe", 0);
            var powrot = new Product("Powrót", "Wróć do poprzedniego menu", 0f, sysCat);

            products.Add(powrot);


            while (true)
            {
                var selectedProduct = AnsiConsole.Prompt(
                    new SelectionPrompt<Product>()
                        .Title("Select a [green]product[/] to add to the cart or [red]press ESC to go back[/]:")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more products)[/]")
                        .AddChoices(products)
                        .UseConverter(p =>
                        {
                            if (p.name == "Powrót" || p.name == "Zamów")
                            {
                                return $"{p.name} ";
                            }
                            else
                            {
                                if(client.wantNetto == false)
                                {
                                    return $"{p.name} - Cena: {p.price:C}";
                                }
                                else
                                {
                                    return $"{p.name} - Cena netto: {(p.price) * ((float)(100 - p.category.vat) / 100):C}";
                                }
                            }

                        })
                );

                if (selectedProduct.name == "Powrót")
                {
                    // Wybrano opcję "Powrót", wychodzimy z pętli
                    break;
                }

                // Dodanie wybranego produktu do koszyka
                client.cart.AddItem(selectedProduct);
                AnsiConsole.MarkupLine($"[green]Added to cart:[/] {selectedProduct.name}");

                // Pytanie, czy użytkownik chce dodać kolejny produkt
                bool addMore = AnsiConsole.Confirm("Do you want to add another product to the cart?");
                if (!addMore)
                {
                    break;
                }
            }
            products.Remove(powrot);
        }



        static void ToggleNetPrices(Client client)
        {
            // Pytanie, czy użytkownik chce ceny netto
            bool wantNetPrices = AnsiConsole.Confirm("Do you want to see [green]net prices[/]?");

            if (wantNetPrices)
            {
                client.wantNetto = true;
                AnsiConsole.MarkupLine("[yellow]Net prices are now enabled.[/]");
            }
            else
            {
                client.wantNetto = false;
                AnsiConsole.MarkupLine("[yellow]Net prices are now disabled.[/]");
            }
        }
    

    


    }
}
