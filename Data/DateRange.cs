namespace BetterBudgetWeb.Data
{
    public class DateRange
    {
        public DateRange() { }
        public DateRange(DateRange other)
        {
            UniqueMonthYears = new List<string>(UniqueMonthYears);
        }
        public List<string> UniqueMonthYears { get; set; }
        public bool IsItValidMonthYear(string monthYear)
        {
            if (monthYear.ToUpper().Contains("ALL")) return true;

            foreach(var year in UniqueMonthYears)
            {
                if (monthYear.ToUpper() == year.ToUpper())
                    return true;
            }
            return false;
        } 
    }
}
