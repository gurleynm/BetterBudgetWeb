namespace BetterBudgetWeb.Data
{
    public class ProjectedDatum
    {
        public string Month { get; set; }
        public int Year { get; set; }
        public double Person1Projected { get; set; }
        public double Person2Projected { get; set; }
        public double TotalProjected => Person1Projected + Person2Projected;
        public double Person1Actual { get; set; } = -1;
        public double Person2Actual { get; set; } = -1;
        public double AnticipatedExpense { get; set; }
        public double TotalActual => Person1Actual + Person2Actual;
        public bool IsProjected { get; set; }
        public ProjectedDatum() { }
        public ProjectedDatum(ProjectedDatum other)
        {
            Month = other.Month;
            Year = other.Year;
            Person1Projected = other.Person1Projected;
            Person2Projected = other.Person2Projected;
            Person1Actual = other.Person1Actual;
            Person2Actual = other.Person2Actual;
        }
        public ProjectedDatum(ProjectedDatum other, string month, int year, bool KeepActual = false)
        {
            Month = month;
            Year = year;
            Person1Projected = other.Person1Projected;
            Person2Projected = other.Person2Projected;
            if (KeepActual)
            {
                Person1Actual = other.Person1Actual;
                Person2Actual = other.Person2Actual;
            }
        }
        public ProjectedDatum(string month, int year, double person1Actual, double person2Actual)
        {
            Month = month;
            Year = year;
            Person1Actual = person1Actual;
            Person2Actual = person2Actual;
        }
        public string MonthYear()
        {
            return Month + " " + Year.ToString();
        }
    }
}
