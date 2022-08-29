namespace BetterBudgetWeb.Data
{
    public class EnvelopeMoney
    {
        public string Id { get; set; }

        public string MoneyBalanceFrom { get; set; }

        public double Person1Amount { get; set; }

        public double Person2Amount { get; set; }

        public double TotalAmount => Person1Amount + Person2Amount;

        public EnvelopeMoney() { }

        public EnvelopeMoney(string Id)
        {
            this.Id = Id;
        }

        public EnvelopeMoney(string Id, string MoneyBalanceFrom, double Person1Amount, double Person2Amount)
        {
            this.Id = Id;
            this.MoneyBalanceFrom = MoneyBalanceFrom;
            this.Person1Amount = Person1Amount;
            this.Person2Amount = Person2Amount;
        }

        public EnvelopeMoney(EnvelopeMoney other)
        {
            this.Id = other.Id;
            this.MoneyBalanceFrom = other.MoneyBalanceFrom;
            this.Person1Amount = other.Person1Amount;
            this.Person2Amount = other.Person2Amount;
        }
    }
}
