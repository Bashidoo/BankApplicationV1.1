using System.Collections.Generic;
using System.Text.Json;

namespace BankApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Bank<int> bank = new Bank<int>();
            Console.WriteLine("Welcome to ABODAN Bank");
            string dataJsonFilePath = "data.json";

            // Debugging ----------
            

            // Debugging ----------

            try
            {
                string allDatafromJsonType = File.ReadAllText(dataJsonFilePath);
                MyDB<int> myDB = JsonSerializer.Deserialize<MyDB<int>>(allDatafromJsonType)!;
                bank.invoices.AddRange(myDB.AllInvoicesFromDB);
                bank.Users.AddRange(myDB.AllUsersFromDB);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            User<int>? CurrentUserSession = null;

            bool running = true;
            while (running)

            {
            Console.WriteLine("Do you want to Create user(1) or Login(2) 1/2 or Exit(3).");
                var choice = Console.ReadLine();
                var menu = new Menu<int>(bank);

                if (choice == "1")
                {
                    menu.AskInfoForUserOBJ();
                }
                else if (choice == "2")
                {

                    int userid = bank.GetValidatedNumberInput("Please write userID");
                    string userpassword = bank.GetValidatedStringInput("Please write userPassword");
                    bank.UserLogin(userid, userpassword, ref CurrentUserSession);

                    if (CurrentUserSession != null)
                    {

                        menu.Show(CurrentUserSession);
                        running = false;
                    }
                    else
                    {
                        Console.WriteLine("Logging in failed.");
                        
                    }
                }
                else if (choice == "3")
                {

                    return;
                    
                }
                else
                {
                    Console.WriteLine("Invalid Input please try again.");
                }
            }


            try
            {
                var myDB = new MyDB<int> { AllUsersFromDB = bank.Users, AllInvoicesFromDB = bank.invoices }; // Collect current state of users
                string updatedDB = JsonSerializer.Serialize(myDB, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(dataJsonFilePath, updatedDB);
                Console.WriteLine("Data saved successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving Json file. \n Reason: {ex.Message}");
            }


        }
    }
}
