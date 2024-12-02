﻿using FluentValidation;
using FluentValidation.Validators;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication
{
    public class User<T> 
    {

        
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }
        public double CardNumber { get; set; }

        public double Balance { get; set; }

        public User<T>? CurrentUserSession { get; set; }

        public List<T> TransactionHistory { get; set; } = new List<T>(); // Add Transaction History logic.


        public User(int id, string name, double cardnumber, double balance,string password) 
        {
            Id = id;
            Name = name;
            CardNumber = cardnumber;
            Balance = balance;
            Password = password;
        }




    }


    public abstract class BaseValidator<T> : AbstractValidator<T> where T : User<T>
    {
        protected BaseValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("ID is required.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.CardNumber).NotEmpty().WithMessage("CardNumber is required.");
            RuleFor(x => x.Balance).NotNull().WithMessage("Initial balance is required.");
            RuleFor(x => x.Balance).NotEmpty().WithMessage("Initial balance is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is Required.");






        }

    }



}
