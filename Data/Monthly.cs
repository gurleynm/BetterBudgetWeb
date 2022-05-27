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
        public string PassKey { get; set; } = "no";
        public double TotalAmount => Person1Amount + Person2Amount;
        public string MonthYear()
        {
            return Month + " " + Year;
        }

        public Monthly() { }
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
