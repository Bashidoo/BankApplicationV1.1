using System.Collections.Generic;
using System.Text.Json;

namespace BankApplication
{
    internal class Program<T>
    {
        static void Main(string[] args)
        {

            Bank<int> bank = new Bank<int>();
            Console.WriteLine("Welcome to ABODAN Bank");
            string dataJsonFilePath = "data.json";

            try
            {
                string allDatafromJsonType = File.ReadAllText(dataJsonFilePath);
                MyDB<int> myDB = JsonSerializer.Deserialize<MyDB<int>>(allDatafromJsonType)!;
                bank.Users.AddRange(myDB.AllUsersFromDB);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            User<int>? CurrentUserSession = null;
            var CurrentSession = bank.UserLoggingin(CurrentUserSession);
            var menu = new Menu<int>(bank);
            menu.Show(CurrentUserSession);

            try
            {
                string allDatafromJsonType = File.ReadAllText(dataJsonFilePath);
                var myDB = new MyDB<int> { AllUsersFromDB = bank.Users };
                string updatedDB = JsonSerializer.Serialize(myDB, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(dataJsonFilePath, updatedDB);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving Json file. \n Reason: {ex.Message}");
            }


        }
    }
}
