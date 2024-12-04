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

        public static Bank<T> _bank;
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

                    Console.WriteLine("1. Add User"); // ---
                    Console.WriteLine("2. Remove User");  // ---
                    Console.WriteLine("3. Search for user"); // Add logic and DisplayUserBalance
                    Console.WriteLine("4. Exit"); // return;
                    Console.WriteLine("5. Select type of account."); // Add logic
                    Console.WriteLine("6. Transfer money."); // Add function on credit card number. 
                    Console.WriteLine("7. Toggle Online payment."); // Attribute
                    Console.WriteLine("8. History of transactions."); // Date.Time List
                    Console.WriteLine("9. Apply for a loan."); // Add function
                    Console.WriteLine("10. Pay Invoice"); // Add Invoice logic with OCR number and Lists. 

                while (running)
                {

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

                            break;
                        case '7':

                            break;
                        case '8':

                            break;



                    }

                }
            }
       

        }


        public void AskInfoForUserOBJ()
        {
            // User(int id, string name, double cardnumber, T balance)
            Bank<double> _bank = new Bank<double>(); // to fix adding user.

            int id = _bank.GetValidatedNumberInput("Please type your ID:");
            string? name = _bank.GetValidatedStringInput("Please type your name:");
            double cardnumber = _bank.GetValidatedNumberInput("Please type your ID:");
            double balance = Convert.ToDouble(Console.ReadLine());
            string? password = _bank.GetValidatedStringInput("Please type your desired password");


            var newUser = new User<double>(id, name, cardnumber, balance, password);

            if (newUser != null)
            {

              _bank.AddUser(newUser);

            }


        }

    }
}
