namespace BetterBudgetWeb.Data
{
    public class Snapshot
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Month { get; set; }
        public int Year { get; set; }
        public double Person1Income { get; set; }
        public double Person2Income { get; set; }
        public int Person1Transactions { get; set; }
        public int Person2Transactions { get; set; }
        public int TotalTransactions { get; set; }
        public double Person1AmountProjected { get; set; }
        public double Person2AmountProjected { get; set; }
        public double Person1AmountActual { get; set; }
        public double Person2AmountActual { get; set; }
        public string DynamicJSON { get; set; }

        public Snapshot() { }

        public Snapshot(Snapshot other)
        {
            Id = other.Id;
            Month = other.Month;
            Year = other.Year;
            Person1Income = other.Person1Income;
            Person2Income = other.Person2Income;
            Person1Transactions = other.Person1Transactions;
            Person2Transactions = other.Person2Transactions;
            TotalTransactions = other.TotalTransactions;
            Person1AmountProjected = other.Person1AmountProjected;
            Person2AmountProjected = other.Person2AmountProjected;
            Person2AmountActual = other.Person2AmountActual;
            Person1AmountActual = other.Person1AmountActual;
            DynamicJSON = other.DynamicJSON;
        }
    }
}
