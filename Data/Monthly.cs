﻿using Stripe.V2;

namespace BetterBudgetWeb.Data
{
    public class Monthly
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public double Person1Amount { get; set; }
        public double Person2Amount { get; set; }
        public string Dynamic { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public double SpentReport { get; set; }
        public double TotalAmount => Person1Amount + Person2Amount;
        public double Left => TotalAmount -   Constants.Transactions.Where(d => 
                                                                d.DateOfTransaction.Month == DateTime.Now.Month &&
                                                                d.DateOfTransaction.Year == DateTime.Now.Year
                                                                && ((Dynamic == "STATIC" && d.ExpenseType == "1-Time Charge" && d.Name == Name) || (Dynamic == "DYNAMIC" && d.ExpenseType == Name)))
                                                                .Sum(c => c.Person1Amount + c.Person2Amount);
        public string MonthYear()
        {
            return Month + " " + Year;
        }

        public Monthly() { }
        public Monthly(string name, double person1, double person2, string dyna)
        {
            Name = name;
            Person1Amount = person1;
            Person2Amount = person2;
            Dynamic = dyna;
        }
        public Monthly(string name, double person1Amount, double person2Amount, string dyna, string montYear)
        {
            string[] splitter = montYear.Split(' ');
            Name = name;
            Person1Amount = person1Amount;
            Person2Amount = person2Amount;
            Dynamic = dyna;
            Month = splitter.Length < 2 ? "All" : splitter[0];
            Year = splitter.Length < 2 ? "1" : splitter[1];
        }

        public Monthly(Monthly other)
        {
            Id = other.Id;
            Name = other.Name;
            Person1Amount = other.Person1Amount;
            Person2Amount = other.Person2Amount;
            Dynamic = other.Dynamic;
            Month = other.Month;
            Year = other.Year;
        }
    }
}
