namespace BetterBudgetWeb.Data
{
    public class Security
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public double NumShares { get; set; }
        public double AvgCost { get; set; }
        public double Cost { get; set; }
        public double Value { get; set; }
        public double StrikePrice { get; set; }
        public string SecType { get; set; }
        public string Person { get; set; }
        public string BalanceFrom { get; set; }
        public DateTime DateOfSecurity { get; set; } = DateTime.Now;
        public string PassKey { get; set; } = "no";
        public Security() { }
        public Security(string name, string type)
        {
            Name = name;
            SecType = type;
        }
        public Security(string name, string type, double numShares)
        {
            Name = name;
            SecType = type;
            NumShares = numShares;
        }

        public async Task SetCost(double cost, double numShares)
        {
            Value = cost * numShares;
        }
        public Security(Security other)
        {
            Id = other.Id;
            Name = other.Name;
            NumShares = other.NumShares;
            AvgCost = other.AvgCost;
            Cost = other.Cost;
            Value = other.Value;
            StrikePrice = other.StrikePrice;
            SecType = other.SecType;
            Person = other.Person;
            DateOfSecurity = other.DateOfSecurity;
            PassKey = other.PassKey;
        }

        public double CalculateReturn()
        {
            return Math.Round((Value - AvgCost * NumShares) /
                                                (NumShares * AvgCost) * 100, 2);
        }
    }
}
