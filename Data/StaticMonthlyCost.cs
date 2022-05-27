namespace BetterBudgetWeb.Data
{
    public class StaticMonthlyCost
    {
        public string Name { get; set; }
        public double Person1Amount { get; set; }
        public double Person2Amount { get; set; }
        public double TotalAmount => Person1Amount + Person2Amount;
        public StaticMonthlyCost() { }
        public StaticMonthlyCost(string Name)
        {
            this.Name = Name;
        }
        public StaticMonthlyCost(string Name, double Person1Amount, double Person2Amount)
        {
            this.Name = Name;
            this.Person1Amount = Person1Amount;
            this.Person2Amount = Person2Amount;
        }
    }
}
