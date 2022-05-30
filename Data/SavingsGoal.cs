namespace BetterBudgetWeb.Data
{
    public class SavingsGoal
    {
        public string Person { get; set; }
        public double Goal { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string MonthYear()
        {
            return Month + " " + Year.ToString();
        }

        public SavingsGoal() { }
        public SavingsGoal(string Person)
        {
            this.Person = Person;
        }
        public SavingsGoal(string Person, double Goal, string Month, string Year)
        {
            this.Person = Person;
            this.Goal = Goal;
            this.Month = Month;
            this.Year = Year;
        }
    }
}
