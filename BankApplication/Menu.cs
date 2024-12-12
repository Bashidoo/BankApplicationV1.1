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
                Console.WriteLine("You must be logged on to access the menu.");
                return;

            }
            else
            {

                while (running)
                {

                    Console.WriteLine($"Hello! {CurrentUserSession.Name}, Welcome to Abo Dan bank!");
                    Console.WriteLine("1. Deposit"); // ---
                    Console.WriteLine("2. Withdraw");  // ---
                    Console.WriteLine("3. Display Balance"); // Add logic and DisplayUserBalance
                    Console.WriteLine("4. Exit, Logout."); // return;
                    Console.WriteLine("5. Display all users information."); // Add logic
                    Console.WriteLine("6. Transfer money."); // Add function on credit card number. 
                    Console.WriteLine("7. Toggle Online payment."); // Attribute
                    Console.WriteLine("8. History of transactions."); // Date.Time List ----------- Work on Logic
                    Console.WriteLine("9. Pay Invoice"); // Add Invoice logic with OCR number and Lists. ---------- Work on logic
                    Console.WriteLine("L. Apply for loan");
                    Console.WriteLine("I. Display all invoices");
                    Console.WriteLine("A. Create new user.");


                    var choice = Console.ReadKey().KeyChar;
                    // Add login page where you can register and login for user. 

                    switch (choice)
                    {
                        case '1':

                            _bank.Deposit(CurrentUserSession);
                            break;
                        case '2':
                            _bank.Withdraw(CurrentUserSession);
                            break;
                        case '3':
                            _bank.DisplayBalance(CurrentUserSession);
                            break;
                        case '4':
                            Console.WriteLine("Exiting application...... Goodbye!");
                            CurrentUserSession = null;
                            running = false;
                            break;
                        case '5':
                            _bank.DisplayEveryUser();
                            break;
                        case '6':
                            _bank.TransferMoneyToUser(CurrentUserSession);
                            break;
                        case '7':
                            _bank.DisplayUserBalance();
                            break;
                        case '8':
                            _bank.DisplayEveryTransaction(CurrentUserSession);
                            break;
                        case '9':
                            _bank.PayInvoice(CurrentUserSession);
                            break;
                        case 'l':
                            _bank.ApplyForLoan(CurrentUserSession);
                            break;
                        case 'i':
                            _bank.DisplayEveryInvoice();
                            break;


                    }

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
