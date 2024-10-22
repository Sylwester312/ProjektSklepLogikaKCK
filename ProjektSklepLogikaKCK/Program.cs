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
            new FigletText("Witaj w sklepie AleTanio!")
                .Centered()
                .Color(Color.Green));

        // Dodanie przycisku "Dalej"
        AnsiConsole.MarkupLine("\n[bold]Nacisnij [green]Enter[/] by kontynuować...[/]");
        Console.ReadLine(); // Czekanie na naciśnięcie klawisza Enter

        // Pobieranie danych użytkownika (imię, nazwisko, e-mail)
        string firstName = AnsiConsole.Ask<string>("Wprowadź [green]Imie[/]:");
        string lastName = AnsiConsole.Ask<string>("Wprowadź [green]Nazwisko[/]:");
        string email = AnsiConsole.Ask<string>("Wprowadź adres [green]Email[/]:");

        Client client = new Client(firstName, lastName, email);

        AnsiConsole.MarkupLine($"\n[bold]Klient stworzony:[/] {client.name} {client.surname} - [blue]{client.mail}[/]\n");

        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Wybierz opcje:")
                    .AddChoices("Koszyk", "Wyszukiwarka", "Lista Produktów", "Ceny Netto/Brutto", "Exit"));

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
                case "Ceny Netto/Brutto":
                    ToggleNetPrices(client);
                    break;

                case "Exit":
                    return;
            }
        }

        static void ShowCart(Client client)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold yellow]Twój koszyk zawiera poniższe produkty:[/]");
            if (client.cart.productsInCart.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]Koszyk jest pusty.[/]");
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
                        .Title("Wybierz [green]produkt[/] do dodania do koszyka:")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Przesuwaj góra i dół żeby pokazać kolejne produkty)[/]")
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
                                    return $"{p.name} - Cena brutto: {p.price:C}";
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
                    AnsiConsole.MarkupLine("[green] Dziękujemy za zamówienie.[/]");
                    break;
                }

                client.cart.DeleteItem(selectedProduct);
                AnsiConsole.MarkupLine($"[green]Usunięto z koszyka:[/] {selectedProduct.name}");

                if (client.cart.productsInCart.Count > 2)
                {
                    bool addMore = AnsiConsole.Confirm("Chcesz usunąć kolejny produkt z koszyka?");
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
                 .Title("Wybierz opcje wyszukiwania:")
                 .AddChoices("Wyszukiwanie po nazwie", "Wyszukiwanie po kategorii"));

            var foundProducts = new List<Product>();

            if (searchOption == "Wyszukiwanie po nazwie")
            {
                string searchTerm = AnsiConsole.Ask<string>("Wpisz nazwę [green]produktu [/] do wyszukiwania:");

                foundProducts = shop.FilterProductsByName(searchTerm);

                if (foundProducts.Count == 0)
                {
                    AnsiConsole.MarkupLine("[red]Nie znaleziono produktu.[/]");
                    return;
                }
            }
            else
            {
                string searchTerm = AnsiConsole.Ask<string>("Wpisz nazwę [green]kategorii [/] do wyszukiwania:");

                foundProducts = shop.FilterProductsByCategory(searchTerm);

                if (foundProducts.Count == 0)
                {
                    AnsiConsole.MarkupLine("[red]Nie znaleziono produktu.[/]");
                    return;
                }
            }



            AnsiConsole.MarkupLine("[bold yellow]Wynik wyszukiwania:[/]");
            foreach (var product in foundProducts)
            {
                if(client.wantNetto == false)
                AnsiConsole.MarkupLine($"[green]{product.name}[/] - {product.price:C}");
                else
                    AnsiConsole.MarkupLine($"[green]{product.name}[/] - {(product.price) * ((float)(100 - product.category.vat) / 100):C}");
            }

            // Pytanie, czy użytkownik chce dodać produkty do koszyka
            bool addProduct = AnsiConsole.Confirm("\nChcesz dodać któryś produkt do koszyka?");
            if (!addProduct)
            {
                return;
            }






            // do tego momentu jest zmiana na polski 







            // Dodawanie produktów do koszyka
            while (true)
            {
                var selectedProduct = AnsiConsole.Prompt(
                    new SelectionPrompt<Product>()
                        .Title("Wybierz [green]produkt[/] do dodania do koszyka:")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Przesuwaj góra i dół żeby pokazać kolejne produkty)[/]")
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
                                    return $"{p.name} - Cena brutto: {p.price:C}";
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
                AnsiConsole.MarkupLine($"[green]Dodano do koszyka:[/] {selectedProduct.name}");

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
                        .Title("Wybierz [green]produkt[/] do dodania do koszyka:")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Przesuwaj góra i dół żeby pokazać kolejne produkty)[/]")
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
                                    return $"{p.name} - Cena brutto: {p.price:C}";
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
                AnsiConsole.MarkupLine($"[green]Dodano do koszyka:[/] {selectedProduct.name}");

                // Pytanie, czy użytkownik chce dodać kolejny produkt
                bool addMore = AnsiConsole.Confirm("Chcesz dodać kolejny produkt do koszyka?");
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
            bool wantNetPrices = AnsiConsole.Confirm("Chcesz zobaczyć ceny [green]Netto[/]?");

            if (wantNetPrices)
            {
                client.wantNetto = true;
                AnsiConsole.MarkupLine("Aktualne ceny: [yellow]Netto[/].");
            }
            else
            {
                client.wantNetto = false;
                AnsiConsole.MarkupLine("Aktualne ceny: [yellow]Brutto[/].");
            }
        }
    

    


    }
}
