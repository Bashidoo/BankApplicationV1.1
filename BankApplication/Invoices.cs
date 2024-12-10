using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication
{
    public class Invoices
    {
        public string CompanyName { get; set; }

        public double OCR { get; set; }

        public double BankGiro { get; set; }

        public double AmountToPay { get; set; }

        public bool IsPayable { get; set; }

        public Invoices(string companyname, double bankgiro, double ocr, double amounttopay) 
        { 
            CompanyName = companyname;
            BankGiro = bankgiro;
            OCR = ocr;
            AmountToPay = amounttopay;
        
        }

    }
}
