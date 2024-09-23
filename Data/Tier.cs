namespace BetterBudgetWeb.Data
{
    public class Tier
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double MonthlyCharge { get; set; }
        public double YearlyCharge { get; set; }
        public string ColorScheme { get; set; }
        public Tier() { }
        public Tier(Tier other)
        {
            Id = other.Id;
            Name = other.Name;
            MonthlyCharge = other.MonthlyCharge;
            YearlyCharge = other.YearlyCharge;
            ColorScheme = other.ColorScheme;
        }
        public Tier(string name, double monthlyCharge, double yearlyCharge, string colorScheme)
        {
            Name = name;
            MonthlyCharge = monthlyCharge;
            YearlyCharge = yearlyCharge;
            ColorScheme = colorScheme;
        }
    }
}
