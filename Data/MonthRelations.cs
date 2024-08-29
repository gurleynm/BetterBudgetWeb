namespace BetterBudgetWeb.Data
{
    public class MonthRelations
    {
        private string[] Months = new string[] { "All", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        public string Name { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public double ProjectPerson1Income { get; set; } = int.MinValue;
        public double ProjectPerson2Income { get; set; } = int.MinValue;
        public int YearMonth => Year * 100 + Month;
        public bool Show { get; set; }
        public MonthRelations(int monthNum, int yearNum)
        {
            Name = Months[monthNum] + " " + yearNum;
            Month = monthNum;
            Year = yearNum;
            Show = false;
        }
    }
}
