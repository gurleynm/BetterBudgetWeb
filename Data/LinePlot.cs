namespace BetterBudgetWeb.Data
{
    public class LinePlot
    {
        public string Month { get; set; }
        public double Amount { get; set; }
        public bool certainty { get; set; }
        public LinePlot(string month, double amount, bool cert = true)
        {
            Month = month;
            Amount = amount;
            certainty = cert;
        }
    }
}
