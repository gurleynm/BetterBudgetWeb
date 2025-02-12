namespace BetterBudgetWeb.Data
{
    public class MonthRelations
    {
        private string[] Months = new string[] { "All", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        public string Name { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public List<ExpenseAmount> Expenses { get; set; }
        public double ProjectPerson1Income { get; set; } = int.MinValue;
        public double ProjectPerson2Income { get; set; } = int.MinValue;
        public int YearMonth => Year * 100 + Month;
        public bool Show { get; set; }
        public MonthRelations() { }
        public MonthRelations(int monthNum, int yearNum, List<ExpenseAmount> expenses)
        {
            Name = Months[monthNum] + " " + yearNum;
            Month = monthNum;
            Year = yearNum;
            Show = false;
            Expenses = expenses;
        }
        public MonthRelations(MonthRelations other)
        {
            Name = other.Name;
            Month = other.Month;
            Year = other.Year;
            Show = other.Show;
            ProjectPerson1Income = other.ProjectPerson1Income;
            ProjectPerson2Income = other.ProjectPerson2Income;
            Expenses = other.Expenses;
        }
    }
}
