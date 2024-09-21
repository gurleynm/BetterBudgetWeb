namespace BetterBudgetWeb.Data
{
    public class DateRange
    {
        public DateRange() { }
        public DateRange(DateRange other)
        {
            if (other.UniqueMonthYears != null)
                UniqueMonthYears = new List<string>(other.UniqueMonthYears);
        }
        public List<string> UniqueMonthYears { get; set; } = new List<string>();
        public bool IsItValidMonthYear(string monthYear)
        {
            if (monthYear.ToUpper().Contains("ALL")) return true;

            foreach (var year in UniqueMonthYears)
            {
                if (monthYear.ToUpper() == year.ToUpper())
                    return true;
            }
            return false;
        }
    }
}
