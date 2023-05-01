using BetterBudgetWeb.Repo;
using System.Transactions;

namespace BetterBudgetWeb.Data
{
    public class DynamicCostItem
    {
        public string Name { get; set; }
        public double Person1Amount { get; set; }
        public double Person2Amount { get; set; }
        public double Amount => Person1Amount + Person2Amount;
        public double Left => Amount - Constants.Transactions.Where(d => (d.ExpenseType == Name || (d.ExpenseType + " (ON)") == Name) &&
                                                                d.DateOfTransaction.Month == DateTime.Now.Month &&
                                                                d.DateOfTransaction.Year == DateTime.Now.Year).Sum(c => c.Person1Amount + c.Person2Amount);
        public double SpentReport { get; set; }
        public DynamicCostItem() { }
        public DynamicCostItem(string Name)
        {
            this.Name = Name;
        }
        public DynamicCostItem(string Name, double Person1Amount, double Person2Amount)
        {
            this.Name = Name;
            this.Person1Amount = Person1Amount;
            this.Person2Amount = Person2Amount;
        }
    }
}
