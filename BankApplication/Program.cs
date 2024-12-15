using System.Collections.Generic;
using Spectre.Console.Rendering;
using System.Text.Json;
using Spectre.Console;



namespace BankApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Bank<int> bank = new Bank<int>();
            string dataJsonFilePath = "data.json";

            
            AnsiConsole.Write(new FigletText("Welcome to ABODAN Bank")
                
                .Color(Color.Aqua));
            

            // Load Data
            try
            {
                string allDatafromJsonType = File.ReadAllText(dataJsonFilePath);
                MyDB<int> myDB = JsonSerializer.Deserialize<MyDB<int>>(allDatafromJsonType)!;
                bank.invoices.AddRange(myDB.AllInvoicesFromDB);
                bank.Users.AddRange(myDB.AllUsersFromDB);
                AnsiConsole.MarkupLine("[green]Data loaded successfully![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error loading data: {ex.Message}[/]");
            }

            User<int>? CurrentUserSession = null; // Declaring user session is null.

            // Main Menu Loop
            bool running = true;
            while (running)
            {
                
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[cyan]What would you like to do?[/]")
                        .AddChoices("Create User", "Login", "Exit"));

                switch (choice)
                {
                    case "Create User":
                        new Menu<int>(bank).AskInfoForUserOBJ();
                        break;

                    case "Login":
                        int userId = bank.GetValidatedNumberInput("Please enter your [yellow]User ID[/]:");
                        string userPassword = bank.GetValidatedStringInput("Please enter your [yellow]Password[/]:");

                        AnsiConsole.Status().Start("Logging in...", ctx =>
                        {
                            bank.UserLogin(userId, userPassword, ref CurrentUserSession);
                        });

                        if (CurrentUserSession != null)
                        {
                            var menu = new Menu<int>(bank);
                            menu.Show(CurrentUserSession);
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Login failed. Please try again.[/]");
                        }
                        break;

                    case "Exit":
                        running = false;
                        AnsiConsole.MarkupLine("[green]Thank you for using ABODAN Bank. Goodbye![/]");
                        break;
                }
            }

            // Save Data
            try
            {
                var myDB = new MyDB<int>
                {
                    AllUsersFromDB = bank.Users,
                    AllInvoicesFromDB = bank.invoices
                };
                string updatedDB = JsonSerializer.Serialize(myDB, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(dataJsonFilePath, updatedDB);
                AnsiConsole.MarkupLine("[green]Data saved successfully![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error saving data: {ex.Message}[/]");
            }
        }
    }
}
