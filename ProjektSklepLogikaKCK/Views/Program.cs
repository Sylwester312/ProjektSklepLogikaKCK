using ProjektSklepLogikaKCK.Controllers;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjektSklepLogikaKCK.DTOs;

namespace ProjektSklepLogikaKCK.Views
{
    class Program
    {
        static void Main(string[] args)
        {
            ShopController shopController = new ShopController();

            // Wyświetlenie strony tytułowej
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new FigletText(shopController.GetShopName())
                    .Centered()
                    .Color(Color.Green));

            // Dodanie przycisku "Dalej"
            AnsiConsole.MarkupLine("\n[bold]Nacisnij [green]Enter[/] by kontynuować...[/]");
            Console.ReadLine(); // Czekanie na naciśnięcie klawisza Enter

            // Pobieranie danych użytkownika (imię, nazwisko, e-mail)
            string firstName = AnsiConsole.Ask<string>("Wprowadź [green]Imie[/]:");
            string lastName = AnsiConsole.Ask<string>("Wprowadź [green]Nazwisko[/]:");
            string email = AnsiConsole.Ask<string>("Wprowadź adres [green]Email[/]:");

            ClientController clientController = new ClientController(firstName, lastName, email);

            AnsiConsole.MarkupLine($"\n[bold]Klient stworzony:[/] {clientController.client.name} {clientController.client.surname} - [blue]{clientController.client.mail}[/]\n");

            while (true)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Wybierz opcje:")
                        .AddChoices("Koszyk", "Wyszukiwarka", "Lista Produktów", "Ceny Netto/Brutto", "Exit"));

                switch (choice)
                {
                    case "Koszyk":
                        ShowCart();
                        break;

                    case "Wyszukiwarka":
                        SearchProduct();
                        break;

                    case "Lista Produktów":
                        ShowProductList();
                        break;
                    case "Ceny Netto/Brutto":
                        ToggleNetPrices();
                        break;

                    case "Exit":
                        return;
                }
            }

            void ShowCart()
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[bold yellow]Twój koszyk zawiera poniższe produkty:[/]");
                if (clientController.CountProductsInCart() == 0)
                {
                    AnsiConsole.MarkupLine("[red]Koszyk jest pusty.[/]");
                    return;
                }
                    

                // Dodanie opcji "Powrót" do listy wyników
                var sysCat = shopController.CreateCategory("Systemowe", 0);
                var powrot = shopController.CreateProduct("Powrót", "Wróć do poprzedniego menu", 0f, sysCat);
                var zamow = shopController.CreateProduct("Zamów", "Finalizacja zamówienia", 0f, sysCat);

                clientController.AddProductToClientCart(powrot);
                clientController.AddProductToClientCart(zamow);

                AnsiConsole.Clear();
                if(clientController.WantNetto() == false)
                {
                    AnsiConsole.MarkupLine($"Wartość koszyka: [green] {clientController.GetFullCartPrice()} zł[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"Wartość koszyka: [green] {clientController.GetFullCartPriceNetto()} zł[/]");
                }

                
                while (true)
                {
                    var selectedProduct = AnsiConsole.Prompt(
                        new SelectionPrompt<ProductDTO>()
                            .Title("Wybierz [green]produkt[/] do usunięcia lub złóż [yellow]zamówienie[/]:")
                            .PageSize(10)
                            .MoreChoicesText("[grey](Przesuwaj góra i dół żeby pokazać kolejne produkty)[/]")
                            .AddChoices(clientController.GetProducts())
                            .UseConverter(p =>
                            {
                                if (p.Name == "Powrót" || p.Name == "Zamów")
                                {
                                    return $"{p.Name} ";
                                }
                                else
                                {
                                    if (clientController.client.wantNetto == false)
                                    {
                                        return $"{p.Name} - Cena brutto: {p.Price:C}";
                                    }
                                    else
                                    {
                                        return $"{p.Name} - Cena netto: {(p.Price) * ((float)(100 - p.Category.vat) / 100):C}";
                                    }
                                }

                            })
                    );

                    if (selectedProduct.Name == "Powrót")
                    {
                        // Wybrano opcję "Powrót", wychodzimy z pętli
                        break;
                    }

                    if (selectedProduct.Name == "Zamów")
                    {
                        clientController.ClearCart();
                        AnsiConsole.Clear();
                        AnsiConsole.MarkupLine("[green]Dziękujemy za zamówienie.[/]");
                        break;
                    }

                    clientController.DeleteProductFromClientCartDTO(selectedProduct);
                    AnsiConsole.MarkupLine($"[green]Usunięto z koszyka:[/] {selectedProduct.Name}");

                    if (clientController.CountProductsInCart() > 2)
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

                
                clientController.DeleteProductFromClientCart(zamow);
                clientController.DeleteProductFromClientCart(powrot);
            }

            void SearchProduct()
            {
                AnsiConsole.Clear();

                var searchOption = AnsiConsole.Prompt(
                     new SelectionPrompt<string>()
                     .Title("Wybierz opcje wyszukiwania:")
                     .AddChoices("Wyszukiwanie po nazwie", "Wyszukiwanie po kategorii"));

                var foundProducts = new List<ProductDTO>();

                if (searchOption == "Wyszukiwanie po nazwie")
                {
                    string searchTerm = AnsiConsole.Ask<string>("Wpisz nazwę [green]produktu [/] do wyszukiwania:");

                    foundProducts = shopController.FilterProductsByNameDTO(searchTerm);

                    if (foundProducts.Count == 0)
                    {
                        AnsiConsole.MarkupLine("[red]Nie znaleziono produktu.[/]");
                        return;
                    }
                }
                else
                {
                    string searchTerm = AnsiConsole.Ask<string>("Wpisz nazwę [green]kategorii [/] do wyszukiwania:");

                    foundProducts = shopController.FilterProductsByCategoryDTO(searchTerm);

                    if (foundProducts.Count == 0)
                    {
                        AnsiConsole.MarkupLine("[red]Nie znaleziono produktu.[/]");
                        return;
                    }
                }



                AnsiConsole.MarkupLine("[bold yellow]Wynik wyszukiwania:[/]");
                foreach (var product in foundProducts)
                {
                    if (clientController.WantNetto() == false)
                        AnsiConsole.MarkupLine($"[green]{product.Name}[/] - {product.Price:C}");
                    else
                        AnsiConsole.MarkupLine($"[green]{product.Name}[/] - {(product.Price) * ((float)(100 - product.Category.vat) / 100):C}");
                }

                // Pytanie, czy użytkownik chce dodać produkty do koszyka
                bool addProduct = AnsiConsole.Confirm("\nChcesz dodać któryś produkt do koszyka?");
                if (!addProduct)
                {
                    return;
                }



                // Dodawanie produktów do koszyka
                while (true)
                {
                    var selectedProduct = AnsiConsole.Prompt(
                        new SelectionPrompt<ProductDTO>()
                            .Title("Wybierz [green]produkt[/] do dodania do koszyka:")
                            .PageSize(10)
                            .MoreChoicesText("[grey](Przesuwaj góra i dół żeby pokazać kolejne produkty)[/]")
                            .AddChoices(foundProducts)
                            .UseConverter(p =>
                            {
                                if (p.Name == "Powrót" || p.Name == "Zamów")
                                {
                                    return $"{p.Name} ";
                                }
                                else
                                {
                                    if (clientController.WantNetto() == false)
                                    {
                                        return $"{p.Name} - Cena brutto: {p.Price:C}";
                                    }
                                    else
                                    {
                                        return $"{p.Name} - Cena netto: {(p.Price) * ((float)(100 - p.Category.vat) / 100):C}";
                                    }
                                }

                            })
                    );

                    // Dodanie wybranego produktu do koszyka
                    clientController.AddProductToClientCartDTO(selectedProduct);
                    AnsiConsole.MarkupLine($"[green]Dodano do koszyka:[/] {selectedProduct.Name}");

                    break;
                }
            }

            void ShowProductList()
            {
                AnsiConsole.Clear();

                var sysCat = shopController.CreateCategory("Systemowe", 0);
                var powrot = shopController.CreateProduct("Powrót", "Wróć do poprzedniego menu", 0f, sysCat);

                shopController.AddProduct(powrot);

                while (true)
                {
                    var selectedProduct = AnsiConsole.Prompt(
                        new SelectionPrompt<ProductDTO>()
                            .Title("Wybierz [green]produkt[/] do dodania do koszyka:")
                            .PageSize(10)
                            .MoreChoicesText("[grey](Przesuwaj góra i dół żeby pokazać kolejne produkty)[/]")
                            .AddChoices(shopController.GetProducts())
                            .UseConverter(p =>
                            {
                                if (p.Name == "Powrót")
                                {
                                    return $"{p.Name} ";
                                }
                                else
                                {
                                    if (clientController.WantNetto() == false)
                                    {
                                        return $"{p.Name} - Cena brutto: {p.Price:C}";
                                    }
                                    else
                                    {
                                        return $"{p.Name} - Cena netto: {(p.Price) * ((float)(100 - p.Category.vat) / 100):C}";
                                    }
                                }

                            })
                    );

                    if (selectedProduct.Name == "Powrót")
                    {
                        break;
                    }

                    // Dodanie wybranego produktu do koszyka
                    clientController.AddProductToClientCartDTO(selectedProduct);
                    AnsiConsole.MarkupLine($"[green]Dodano do koszyka:[/] {selectedProduct.Name}");

                    // Pytanie, czy użytkownik chce dodać kolejny produkt
                    bool addMore = AnsiConsole.Confirm("Chcesz dodać kolejny produkt do koszyka?");
                    if (!addMore)
                    {
                        AnsiConsole.Clear();
                        break;
                    }
                }
                shopController.DeleteProduct(powrot);
            }



            void ToggleNetPrices()
            {
                AnsiConsole.Clear();
                // Pytanie, czy użytkownik chce ceny netto
                bool wantNetPrices = AnsiConsole.Confirm("Chcesz zobaczyć ceny [green]Netto[/]?");

                if (wantNetPrices)
                {
                    clientController.ChangeVAT(wantNetPrices);
                    AnsiConsole.MarkupLine("Aktualne ceny: [yellow]Netto[/].");
                }
                else
                {
                    clientController.ChangeVAT(wantNetPrices);
                    AnsiConsole.MarkupLine("Aktualne ceny: [yellow]Brutto[/].");
                }
               
            }

        }
    }
}
