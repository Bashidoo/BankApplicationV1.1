using Stripe.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication
{
    public class Transaction
    {
        

        public DateTime DateTime { get; set; }

        public double Amount { get; set; }

        public string Description { get; set; }

        public string NameOfUser { get; set; }

        public double AfterBalance { get; set; }
       


        public Transaction(string description, string nameofuser, DateTime datetime, double amount, double afterBalance)
        {
            
            DateTime = datetime;
            Amount = amount;
            Description = description;
            NameOfUser = nameofuser;
            AfterBalance = afterBalance;
        }

        public override string ToString()
        {
            return $"{DateTime:G}: {Description} of {Amount:C2}. New Balance {AfterBalance}";
        }
    }
}
