using Spectre.Console;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BankApplication
{
    public class Bank<T>
    {
        public List<User<T>> Users { get; set; } = new List<User<T>>();
        public List<Invoices> invoices { get; set; } = new List<Invoices>();
        


        public User<T>? UserLogin(int id, string passWord, ref User<T>? CurrentUserSession)
        {
            
            bool running = true;
            while (running)
            {

                var user = Users.FirstOrDefault(x => x.Id == id && x.Password == passWord);

                if (user != null)
                {
                    if (user.Password == passWord && user.Id == id)
                    {
                        CurrentUserSession = user;
                        running = false;
                        return user;
                        

                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[red]Incorrect details, please try another password or username.[/]");
                        return null;
                    }
                    
                }


                if (user == null)
                {
                    AnsiConsole.MarkupLine($"[red]User not found Please try again.[/]");

                    return null;

                }
               


            }
            return null ;
        }
        public void AddUser(User<T> user)
        {
            if (user == null)
            {
                AnsiConsole.MarkupLine($"[red]Adding User Operation failed[/]");
                return;
            }

            

            else
            {
                Users.Add(user);
                AnsiConsole.MarkupLine($"[green] User Added Successfully[/]");
            }

        }

        public void RemoveUser(int id)
        {
            User<T>? user = GetUserByIdGeneric(id);
            if (user != null) 
            {
                Users.Remove(user);
                AnsiConsole.MarkupLine($"[green] User removed![/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]User not found.[/]");
            }
            
        }

        public void DisplayUserBalance()
        {
            double cardnumber = GetValidatedDoubleNumberInput("Enter cardnumber to identify reciever account.[/]");

            var userToFind = Users.Where(b => b.CardNumber == cardnumber); // Since the List has a generic attribute. It Cannot be  fetched from a single Linq Find, it should be FindALL because it returns a List<User>.
            if (userToFind.Any())
            {
                foreach (var user in userToFind)
                {
                    AnsiConsole.MarkupLine($"[green]Name: {user.Name} \n ID:{user.Id}\n Balance: {user.Balance}[/]");
                }

            }
            else
            {

                AnsiConsole.MarkupLine($"[red]Reciever account not found![/]");
                
            }
        }

        public void DisplayBalance(User<T> CurrentUserSession)
        {
            Console.WriteLine($"Hello! {CurrentUserSession.Name}!\nYour Account balance is: {CurrentUserSession.Balance}");
            
        }

        public void GetUserById(int id)
        {
            if (Users == null || !Users.Any())
            {
                Console.WriteLine("No User is found with current ID. ID is available.");
                return;
            }
            
            var userToFind = Users.SingleOrDefault(x =>  x.Id == id); // Selecting the user in the list.

            if (userToFind != null)
            {
                Console.WriteLine($"User with Name: {userToFind.Name} \nID: {userToFind.Id} \nCardNumber: {userToFind.CardNumber} \nBalance: {userToFind.Balance}.");
            }
            else
            {
                Console.WriteLine("User not found!");
            }

        }

        public User<T>? GetUserByIdGeneric(int id)
        {
            var userToFind = Users.FirstOrDefault(x => x.Id == id); // Selecting the user in the list.

            if (userToFind != null)
            {
                AnsiConsole.MarkupLine($"[green] {userToFind.Name}, ID: {userToFind.Id}[/]");
                return userToFind;
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]User not found![/]");
                return null;
            }
            
        }


        public void TransferMoneyToUser(User<T> CurrentUserSession)
        {
            int recieverCardNumber = GetValidatedNumberInput("Please type cardnumber of the account you want to send money to. \n");

            try
            {
                // Can write FirstOrDefault, just for variance sake.

                
                var RecieverAccount = Users.FirstOrDefault(x => x.CardNumber == recieverCardNumber);
                
               
                if (RecieverAccount != null)
                {
                      double amount = GetValidatedDoubleNumberInput("\n Please enter amount of money you wish to transfer.");
                      if (amount > 0 && CurrentUserSession.Balance > amount)
                      {
                        CurrentUserSession.Balance -= amount;
                        RecieverAccount.Balance += amount;

                      }
                      else
                      {
                        AnsiConsole.MarkupLine($"[red]Insufficient funds, please type an eligible number according to your balance.[/]");
                      }


                }
                    

            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
            }
        }

        public void Deposit(User<T> CurrentUserSession) 
        {
          

             double amount = GetValidatedDoubleNumberInput("\n Enter deposit amount: ");
            try // ask teacher for assistance in how to handle string exceptions
            {
                if (amount > 0)
                {
                    CurrentUserSession.Balance += amount;
                    AnsiConsole.MarkupLine($"[green]Deposited: {amount:C}, New Balance: {CurrentUserSession.Balance:C}[/]");

                    CurrentUserSession.LogTransaction("Deposit", amount, CurrentUserSession.Balance, CurrentUserSession.Name);
                }
                else if (amount <= 0)
                {
                    AnsiConsole.MarkupLine($"[red]Desired amount: {amount:C}, cannot be desposited[/]");
                }

            }

            catch (Exception e) // ask teacher for assistance in how to handle string exceptions
            {
                AnsiConsole.MarkupLine($"[red]Wrong Input, please type a number, reason for error:[/]");
                Console.Write(e.Message);
            }



        }

        public void Withdraw(User<T> CurrentUserSession)
        {
            Console.WriteLine();
            Console.WriteLine("Enter withdrawal amount: ");


            try
            {
                double amount = Convert.ToDouble(Console.ReadLine());
                if (amount > 0)
                {
                    if (amount <= CurrentUserSession.Balance)
                    {
                        CurrentUserSession.Balance -= amount;
                        AnsiConsole.MarkupLine($"[green]Withdrawn: {amount:C}, New Balance: {CurrentUserSession.Balance:C}[/]");

                        CurrentUserSession.LogTransaction("Withdraw", amount, CurrentUserSession.Balance, CurrentUserSession.Name);
                    }
                    else // Denna else gäller för if loop övanför
                    {
                        AnsiConsole.MarkupLine($"[red]Insufficient money.[/]");
                    }
                }
                else // gäller för först If loop om x > 0
                {
                    AnsiConsole.MarkupLine($"[red] Invalid Input please enter a number higher than zero[/]");
                }

            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"[red]Wrong Input, please type a number, reason for error:[/]");
                AnsiConsole.MarkupLine($"[red]{e.Message}[/]");
            }

        }

        static void CheckBalance(User<T> user)
        {
            Console.WriteLine();
            Console.WriteLine($"Current Balance: {user.Balance:C}");
        }
        public int GetValidatedNumberInput(string prompt)
        {
            int number;
            while (true)
            {
                AnsiConsole.Markup($"{prompt} ");

                string? input = Console.ReadLine();
                try
                {

                    if (string.IsNullOrEmpty(input))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        AnsiConsole.MarkupLine("[red]You have entered nothing.[/]");
                        Console.ResetColor();
                    }
                    else
                    {
                        number = Convert.ToInt32(input);
                        return number;
                    }

                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    AnsiConsole.MarkupLine("[red]Invalid input. Please enter a valid number.[/]");
                    Console.ResetColor();
                }
                catch (OverflowException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    AnsiConsole.MarkupLine("[red]Number is too large.[/]");
                    Console.ResetColor();
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    AnsiConsole.MarkupLine("[red]Error: {e.Message}\n[/]");
                    Console.ResetColor();
                }

            }

        }

        public double GetValidatedDoubleNumberInput(string prompt)
        {
            double number;
            while (true)
            {
                AnsiConsole.Markup($"{prompt} ");

                string? input = Console.ReadLine();
                try
                {

                    if (!string.IsNullOrEmpty(input))
                    {

                        number = Convert.ToInt32(input);
                        return number;
                       
                    }


                }
                catch (Exception ex)
                {
                    // Escape exception message to prevent markup errors
                    string escapedMessage = Markup.Escape(ex.Message);
                    AnsiConsole.MarkupLine($"[red]Error: {escapedMessage}[/]");
                }

            }

        }

        public string GetValidatedStringInput(string prompt)
        {

            string convertedString;
            while (true)
            {
                AnsiConsole.Markup($"{prompt} ");

                string? input = Console.ReadLine();
                try
                {

                    if (string.IsNullOrEmpty(input))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        AnsiConsole.MarkupLine("[red]You have entered nothing.[/]");
                        Console.ResetColor();
                    }
                    else
                    {
                        convertedString = Convert.ToString(input);
                        return convertedString;
                    }

                }
                catch (FormatException)
                {
                   
                    AnsiConsole.MarkupLine("[red]Please Enter a valid input.[/]");
                
                }
                catch (OverflowException)
                {
                    
                    AnsiConsole.MarkupLine("[red]Number is too large.[/]");
                    
                }
                catch (Exception e)
                {
                    
                    AnsiConsole.MarkupLine("[red]Error: {e.Message}\n[/]");
                  
                }

            }

        }

        public void DisplayEveryUser()
        {
            if (Users != null) // (Users.Any())
            {
                foreach (var user in Users)
                {


                    AnsiConsole.MarkupLine($"[green]ID:{user.Id}\n Name: {user.Name}\n CardNumber: {user.CardNumber}\n Balance: {user.Balance} \n Creditscore: {user.CreditScore}[/]");
                    
                }

            }
            else
            {
                
                AnsiConsole.MarkupLine($"[red]No Users have been found.[/]");
                
            }
        }
        public void AddInvoice(User<T> CurrentUserSession)
        {
            string companyname = GetValidatedStringInput("Type Company Name:");
            double bankgiro = GetValidatedDoubleNumberInput("Type BankGiro:");
            double amountTopay = GetValidatedDoubleNumberInput("Type amount to pay");
            double ocr = GetValidatedDoubleNumberInput("Type OCR or Reference number:");

            var newInvoice = new Invoices(companyname,bankgiro, amountTopay, ocr);

            var checkinvoice = invoices.FirstOrDefault(x => x.OCR == ocr);

            if (checkinvoice == null)
            {
                invoices.Add(newInvoice);
                AnsiConsole.MarkupLine($"[green] Invoice has been created successfully![/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Invoice with same OCR Number found, please try anothe number.[/]");
            }
        }

        public void DisplayEveryTransaction(User<T> CurrentUserSession)
        {
            if (CurrentUserSession.TransactionList != null)
            {

                AnsiConsole.MarkupLine($"[green]{CurrentUserSession.TransactionList}[/]");


            }
            else
            {
                
                AnsiConsole.MarkupLine($"[red]No transactions have been found.[/]");
                
            }
        }

        public void DisplayTransactions(User<T> CurrentUserSession)
        {
            // Add Logic
        }
        public void DisplayEveryInvoice()
        {
            if (invoices.Any())
            {
                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .AddColumn("[yellow]Company Name[/]")
                    .AddColumn("[yellow]Bank Giro[/]")
                    .AddColumn("[yellow]OCR[/]")
                    .AddColumn("[yellow]Amount To Pay[/]")
                    .AddColumn("[yellow]Is Payable[/]");

                foreach (var invoice in invoices)
                {
                    table.AddRow(
                        invoice.CompanyName,
                        invoice.BankGiro.ToString(),
                        invoice.OCR.ToString(),
                        invoice.AmountToPay.ToString("C"),
                        invoice.IsPayable ? "[green]Yes[/]" : "[red]No[/]");
                }

                AnsiConsole.Write(table);
            }
            else
            {
                AnsiConsole.MarkupLine("[red]No invoices found![/]");
            }
        }


        public void ApplyForLoan(User<T> CurrentUserSession)
        {
            if (CurrentUserSession.CreditScore >= 750)
            {
                double amoutOfMoneyAsked = GetValidatedDoubleNumberInput("What is the required amount of money are you applying for?");
                LoanCalculation(amoutOfMoneyAsked, CurrentUserSession);

            }
            else
            {
                Console.WriteLine($"User is not eligible for loan. Minimum credit score required to apply is 750. Please pay more invoices on time.\n Your credit score: {CurrentUserSession.CreditScore}");
            }
        }

        public void LoanCalculation(double amoutOfMoneyAsked, User<T> CurrentUserSession)
        {
            const double CreditScoreValue = 1000.0;
            int requiredCreditScorePoints = (int)Math.Ceiling(amoutOfMoneyAsked / CreditScoreValue);

            if (CurrentUserSession.CreditScore >= requiredCreditScorePoints)
            {
                CurrentUserSession.CreditScore -= requiredCreditScorePoints;
                 CurrentUserSession.Balance += amoutOfMoneyAsked;
                Console.WriteLine($"Operation Successful! New account balance is {CurrentUserSession.Balance}");
               

            }
            else if (CurrentUserSession.CreditScore < requiredCreditScorePoints)
            {
                Console.WriteLine($"The amount you asked is too large for your credit score to handle.\n You have asked for: {amoutOfMoneyAsked} \n Your CreditScore: {CurrentUserSession.CreditScore}\n Required CreditScore: {requiredCreditScorePoints}");
                
            }
            

        }
        public void PayInvoice(User<T> CurrentUserSession)
        {

            double bankgiro = GetValidatedDoubleNumberInput(" Type BankGiro of following Invoice");
            double amounttopay = GetValidatedDoubleNumberInput(" Type amount.");
            double ocr = GetValidatedDoubleNumberInput("Type OCR or reference.");

            var invoiceToPay = invoices.FirstOrDefault(x => x.BankGiro == bankgiro && x.AmountToPay == amounttopay && x.OCR == ocr);

            if (invoiceToPay != null)
            {
                if (invoiceToPay.IsPayable == true)
                {

                if (CurrentUserSession.Balance > invoiceToPay.AmountToPay)
                {

                    CurrentUserSession.Balance -= invoiceToPay.AmountToPay;
                    invoiceToPay.IsPayable = false;
                        double CreditScoreAfterPayingInvoice = invoiceToPay.AmountToPay / 10;
                        double doubleUserCredit = CurrentUserSession.CreditScore;
                        doubleUserCredit = + CreditScoreAfterPayingInvoice;
                        AnsiConsole.MarkupLine($"[green]Invoice have been successfully paid![/]");
                }

                }
                else
                {
                    AnsiConsole.MarkupLine($"[red] This invoice is already paid![/]");
                }



            }
            else
            {
                AnsiConsole.MarkupLine($"[red]No invoice found to pay.[/]");
               
            }
                // var user = Users.FirstOrDefault(x => x.Id == id && x.Password == passWord);
        }

    }
}
