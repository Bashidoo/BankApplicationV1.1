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
        public void Show()
        {
            // Add login page where you can register and login for user. 
            Console.WriteLine("1. Add User");
            Console.WriteLine("2. Remove User");
            Console.WriteLine("3. Search for user");
            Console.WriteLine("4. Exit");
            Console.WriteLine("5. Select type of account.");
            Console.WriteLine("6. Transfer money."); // Add function to 
            Console.WriteLine("7. Toggle Online payment.");
            Console.WriteLine("8. History of transactions.");
            Console.WriteLine("9. Apply for a loan.");

            User<T>? CurrentUserSession = null;
            CurrentUserSession = UserLoggingin(CurrentUserSession);

          if (CurrentUserSession != null)
          {
            while (running)
            {
                var choice = Convert.ToChar(Console.ReadKey());

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
          else
            {
                Console.WriteLine("Logging in failed. Exiting program");
                return;
            }



            

        }


        public void AskInfoForUserOBJ()
        {
            // User(int id, string name, double cardnumber, T balance)

            int id = Convert.ToInt32(Console.ReadLine());
            string? name = Convert.ToString(Console.ReadLine());
            double cardnumber = Convert.ToDouble(Console.ReadLine());
            double balance = Convert.ToDouble(Console.ReadLine());

            Bank<double> _bank = new Bank<double>(); // to fix adding user.

            var newUser = new User<double>(id, name, cardnumber, balance);

            if (newUser != null)
            {

            _bank.AddUser(newUser);

            }


        }

        public User<T>? UserLoggingin(User<T> CurrentUserSession)
        {
            int userid = _bank.GetValidatedNumberInput("Please type User ID to Login:");
            string userpassword = _bank.GetValidatedStringInput("Please type user password:");
            var user = _bank.UserLogin(userid, userpassword);

            if (user != null)
            {
                Console.WriteLine("User logged in succesfully.");
            return user = CurrentUserSession;

            }
            return null;
        }
    }
}
