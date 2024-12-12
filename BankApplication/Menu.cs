using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication
{
    public class Menu<T>
    {
            bool running = true;

        private Bank<T> _bank;
        public Menu(Bank<T> bank) 
        {
            _bank = bank;
        }
        public void Show(User<T> CurrentUserSession)
        {
            if (CurrentUserSession == null)
            {
                AnsiConsole.MarkupLine("[red]You must be logged in to access the menu.[/]");
                return;
            }

            while (running)
            {
                AnsiConsole.MarkupLine($"[green]Hello, {CurrentUserSession.Name}! Welcome to ABODAN Bank![/]");
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[cyan]Select an option[/]:")
                        .AddChoices(
                            "Deposit",
                            "Withdraw",
                            "Display Balance",
                            "Display All Users",
                            "Transfer Money",
                            "Toggle Online Payment",
                            "Transaction History",
                            "Pay Invoice",
                            "Display All Invoices",
                            "Exit"));

                switch (choice)
                {
                    case "Deposit":
                        _bank.Deposit(CurrentUserSession);
                        break;

                    case "Withdraw":
                        _bank.Withdraw(CurrentUserSession);
                        break;

                    case "Display Balance":
                        _bank.DisplayBalance(CurrentUserSession);
                        break;

                    case "Display All Users":
                        _bank.DisplayEveryUser();
                        break;

                    case "Transfer Money":
                        _bank.TransferMoneyToUser(CurrentUserSession);
                        break;

                    case "Transaction History":
                        _bank.DisplayEveryTransaction(CurrentUserSession);
                        break;

                    case "Pay Invoice":
                        _bank.PayInvoice(CurrentUserSession);
                        break;

                    case "Display All Invoices":
                        _bank.DisplayEveryInvoice();
                        break;

                    case "Exit":
                        AnsiConsole.MarkupLine("[green]Logging out... Goodbye![/]");
                        running = false;
                        break;
                }
            }
        }



        public void AskInfoForUserOBJ()
        {
            // User(int id, string name, double cardnumber, T balance)
            

            int id = _bank.GetValidatedNumberInput("Please type your ID:");
            string? name = _bank.GetValidatedStringInput("Please type your name:");
            double cardnumber = _bank.GetValidatedNumberInput("Please type your desired cardnumber:");
            double balance = _bank.GetValidatedDoubleNumberInput("Please type desired balance.");
            string? password = _bank.GetValidatedStringInput("Please type your desired password");


            var newUser = new User<T>(id, name, cardnumber, balance, password);

            if (newUser != null)
            {

                _bank.AddUser(newUser);

            }


        }

    }
}
