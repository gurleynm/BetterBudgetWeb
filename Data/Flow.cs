namespace BetterBudgetWeb.Data
{
    public class Flow
    {
        public string From { get; set; }
        public string To { get; set; }
        public double Amount { get; set; }
        public Flow() { }
        public Flow(string from, string to, double total) { From = from; To = to; Amount = total; }
    }
}
