﻿using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication
{
    public class Bank<T>
    {
        public List<User<T>> Users { get; set; } = new List<User<T>>();




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
                        Console.WriteLine("Incorrect details, please try another password or username.");
                        return null;
                    }
                    
                }


                if (user == null)
                {
                    Console.WriteLine("User not found Please try again.");

                    return null;

                }
               


            }
            return null ;
        }
        public void AddUser(User<T> user)
        {
            if (user == null)
            {
                Console.WriteLine("Adding User Operation failed");
                return;
            }
            else
            {
                Console.WriteLine("User Added Successfully");
                Users.Add(user);
            }

        }

        public void RemoveUser(int id)
        {
            User<T>? user = GetUserByIdGeneric<T>(id);
            if (user != null) 
            {
                Users.Remove(user); 
            }
            else
            {
                Console.WriteLine("User not found.");
            }
            
        }

        public void DisplayUserBalance(double cardnumber)
        {
            var userToFind = Users.Where(b => b.CardNumber == cardnumber); // Since the List has a generic attribute. It Cannot be  fetched from a single Linq Find, it should be FindALL because it returns a List<User>.
            if (userToFind.Any())
            {
                foreach (var user in userToFind)
                {
                    Console.WriteLine($" Name: {user.Name} \n ID:{user.Id}\n Balance: {user.Balance}");
                }

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
                Console.WriteLine($"User with Name: {userToFind.Name}, ID: {userToFind.Id}");
            }
            else
            {
                Console.WriteLine("User not found!");
            }

        }

        public User<T>? GetUserByIdGeneric<T>(int id)
        {
            var userToFind = Users.OfType<User<T>>().FirstOrDefault(x => x.Id == id); // Selecting the user in the list.

            if (userToFind != null)
            {
                Console.WriteLine($"User with Name: {userToFind.Name}, ID: {userToFind.Id}");
                return userToFind;
            }
            else
            {
                Console.WriteLine("User not found!");
                return null;
            }
            
        }

        public void TransferMoneyToUser(User<T> CurrentUserSession)
        {
            int recieverCardNumber = GetValidatedNumberInput("Please type cardnumber of the account you want to send money to. \n");

            try
            {
                var RecieverAccount = Users.Where(x => x.Id == recieverCardNumber).ToList();
                if (RecieverAccount.Any())
                {
                    foreach (var user in RecieverAccount)
                    {

                      double amount = GetValidatedDoubleNumberInput("\n Please enter amount of money you wish to transfer.");
                      if (amount > 0)
                      {
                        CurrentUserSession.Balance -= amount;
                        user.Balance += amount;

                      }

                    }
                }
                else
                {
                    Console.WriteLine("Reciever Account not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Deposit(User<T> CurrentUserSession) // Void was chosen because of no value returning due to lack of inheritance.
        {
          

             double amount = GetValidatedDoubleNumberInput("\n Enter deposit amount: ");
            try // ask teacher for assistance in how to handle string exceptions
            {
                if (amount > 0)
                {
                    CurrentUserSession.Balance += amount;
                    Console.WriteLine($"Deposited: {amount:C}, New Balance: {CurrentUserSession.Balance:C}");


                }
                else if (amount <= 0)
                {
                    Console.Write($"Desired amount: {amount:C}, cannot be desposited");
                }

            }

            catch (Exception e) // ask teacher for assistance in how to handle string exceptions
            {
                Console.WriteLine("Wrong Input, please type a number, reason for error:");
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
                         // balance -= amount;
                        Console.WriteLine($"Withdrawn: {amount:C}, New Balance: {CurrentUserSession.Balance:C}");
                    }
                    else // Denna else gäller för if loop övanför
                    {
                        Console.WriteLine("Insufficient money.");
                    }
                }
                else // gäller för först If loop om x > 0
                {
                    Console.WriteLine(" Invalid Input please enter a number higher than zero");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Wrong Input, please type a number, reason for error:");
                Console.Write(e.Message);
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
                Console.Write(prompt);

                string? input = Console.ReadLine();
                try
                {

                    if (string.IsNullOrEmpty(input))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You have entered nothing.");
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
                    Console.WriteLine("Please Enter a valid input.");
                    Console.ResetColor();
                }
                catch (OverflowException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Number is too large.");
                    Console.ResetColor();
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {e.Message}");
                    Console.ResetColor();
                }

            }

        }

        public double GetValidatedDoubleNumberInput(string prompt)
        {
            double number;
            while (true)
            {
                Console.Write(prompt);

                string? input = Console.ReadLine();
                try
                {

                    if (string.IsNullOrEmpty(input))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You have entered nothing.");
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
                    Console.WriteLine("Please Enter a valid input.");
                    Console.ResetColor();
                }
                catch (OverflowException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Number is too large.");
                    Console.ResetColor();
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {e.Message}");
                    Console.ResetColor();
                }

            }

        }

        public string GetValidatedStringInput(string prompt)
        {

            string convertedString;
            while (true)
            {
                Console.Write(prompt);

                string? input = Console.ReadLine();
                try
                {

                    if (string.IsNullOrEmpty(input))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You have entered nothing.");
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please Enter a valid input.");
                    Console.ResetColor();
                }
                catch (OverflowException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Number is too large.");
                    Console.ResetColor();
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {e.Message}");
                    Console.ResetColor();
                }

            }

        }

        public void DisplayEveryUser()
        {
            if (Users != null) // (Users.Any())
            {
                foreach (var user in Users)
                {

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"ID:{user.Id}\n Name: {user.Name}\n CardNumber: {user.CardNumber}\n Balance: {user.Balance}");
                    Console.ResetColor();
                }

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No Users have been found.");
                Console.ResetColor();
            }
        }

    }
}
