using FluentValidation;
using Stripe;
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
            IsPayable = true;
        
        }
        public class InvoiceValidator : AbstractValidator<Invoices>
        {
            public InvoiceValidator()
            {
                RuleFor(n => n.CompanyName).NotEmpty().WithMessage("Company Name is required!")
                .Matches("^[a-zA-Z ]*$").WithMessage("Name must contain only alphabets and spaces!");
               // RuleFor(x => x.BankGiro)
               //   .NotEmpty().WithMessage("BankGiro number is required.")
               // .Matches(@"^\d{3,4}-\d{4}$").WithMessage("BankGiro number must be in the format XXX-XXXX or XXXX-XXXX."); This rule is only applied for strings.



            }
        }
    }
}
