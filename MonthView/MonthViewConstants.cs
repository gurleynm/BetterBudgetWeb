using BetterBudgetWeb.Data;

namespace BetterBudgetWeb.MonthView
{
    public class MonthViewConstants
    {
        public static List<DynamicCostItem> DynamicCostItems { get; set; }
        public static List<SavingsGoal> SavingsGoals { get; set; }
        public static List<StaticMonthlyCost> StaticMonthlyCosts { get; set; }
        public static List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public static List<Balance> Balances { get; set; } = new List<Balance>();
        public static List<ProjectedDatum> ProjectedData { get; set; }
        public static List<MonthViewPrev> MonthViewPrevs { get; set; } = new List<MonthViewPrev>();
        public static double TotalPerson1Expenses => GetTotalExpenses(Constants.Person1);
        public static double TotalPerson2Expenses => GetTotalExpenses(Constants.Person2);
        public static double TotalExpenses => GetTotalExpenses();

        public static int Month { get; set; } = DateTime.Now.Month;
        public static int Year { get; set; } = DateTime.Now.Year;

        public static string PassKey { get; set; } = "no";
        public static async Task Init()
        {
            SetMonthlies(DateTime.Now.Month, DateTime.Now.Year);
        }
        public static List<Monthly> GetNeededMonthlies(int month, int year)
        {
            Month = month;
            Year = year;

            var DesiredMonthlies = new List<Monthly>();
            Monthly NewMonthly;

            foreach (var mon in Constants.Monthlies)
            {
                if (mon.Dynamic == "DYNAMIC")
                {
                    if (CheckMonthYear(mon))
                    {
                        var DynamicMonthly = DesiredMonthlies.FirstOrDefault(d => d.Name == mon.Name);
                        NewMonthly = new Monthly(mon);

                        if (DynamicMonthly == null)
                            DesiredMonthlies.Add(NewMonthly);
                        else
                        {
                            if (mon.Month != "All")
                            {
                                DesiredMonthlies.Remove(DynamicMonthly);
                                DesiredMonthlies.Add(NewMonthly);
                            }
                        }
                    }
                }
                else if (mon.Dynamic == "STATIC")
                {
                    if (CheckMonthYear(mon))
                    {
                        var StaticMonthly = DesiredMonthlies.FirstOrDefault(s => s.Name == mon.Name);
                        NewMonthly = new Monthly(mon);

                        if (StaticMonthly == null)
                            DesiredMonthlies.Add(NewMonthly);
                        else
                        {
                            if (mon.Month != "All")
                            {
                                DesiredMonthlies.Remove(StaticMonthly);
                                DesiredMonthlies.Add(NewMonthly);
                            }
                        }
                    }
                }
            }

            return DesiredMonthlies;
        }

        public static void SetMonthlies(int month, int year)
        {
            Month = month;
            Year = year;

            DynamicCostItems = new List<DynamicCostItem>();
            SavingsGoals = new List<SavingsGoal>();
            StaticMonthlyCosts = new List<StaticMonthlyCost>();
            ProjectedData = new List<ProjectedDatum>();

            foreach (var mon in Constants.Monthlies)
            {
                if (mon.Dynamic == "DYNAMIC")
                {
                    if (CheckMonthYear(mon))
                    {
                        var DCIExists = DynamicCostItems.FirstOrDefault(d => d.Name == mon.Name);
                        var NewDCI = new DynamicCostItem(mon.Name, mon.Person1Amount, mon.Person2Amount);

                        if (DCIExists == null)
                            DynamicCostItems.Add(NewDCI);
                        else
                        {
                            if (mon.Month != "All")
                            {
                                DynamicCostItems.Remove(DCIExists);
                                DynamicCostItems.Add(NewDCI);
                            }
                        }
                    }
                }
                else if (mon.Dynamic == "STATIC")
                {
                    if (CheckMonthYear(mon))
                    {
                        var SMCExists = StaticMonthlyCosts.FirstOrDefault(s => s.Name == mon.Name);
                        var NewSMC = new StaticMonthlyCost(mon.Name, mon.Person1Amount, mon.Person2Amount);

                        if (SMCExists == null)
                            StaticMonthlyCosts.Add(NewSMC);
                        else
                        {
                            if (mon.Month != "All")
                            {
                                StaticMonthlyCosts.Remove(SMCExists);
                                StaticMonthlyCosts.Add(NewSMC);
                            }
                        }
                    }
                }
                else if (mon.Dynamic == "SAVINGS")
                {
                    SavingsGoals.Add(new SavingsGoal(Constants.Person1, mon.Person1Amount, mon.Month, mon.Year));
                    SavingsGoals.Add(new SavingsGoal(Constants.Person2, mon.Person2Amount, mon.Month, mon.Year));
                }
                else if (mon.Dynamic == "PROJECTED DATA")
                {
                    bool tp = int.TryParse(mon.Year, out int yearp);
                    if (!tp) yearp = DateTime.Now.Year;

                    ProjectedDatum pd = new ProjectedDatum(mon.Month, yearp, mon.Person1Amount, mon.Person2Amount);

                    ProjectedData.Add(pd);
                }
            }

            DynamicCostItems = DynamicCostItems.OrderByDescending(dci => dci.Amount).ToList();
            StaticMonthlyCosts = StaticMonthlyCosts.OrderByDescending(smc => smc.TotalAmount).ToList();
            SavingsGoals = SavingsGoals.OrderByDescending(sg => sg.Goal).ToList();
        }
        public static string MonthYear() { return new DateTime(Year, Month, 1).ToString("MMMM") + " " + Year.ToString(); }
        public static string MonthYear(int month, int year) { return new DateTime(year, month, 1).ToString("MMMM") + " " + year.ToString(); }

        public static bool CheckMonthYear(Monthly mon)
        {
            string currentMonthYear = MonthYear();
            string mCM = mon.MonthYear();
            string[] splitter = mCM.Split(" ");

            string month = currentMonthYear.Split(" ")[0];
            string year = DateTime.Now.Year.ToString();
            bool everyYear = false;

            if (splitter.Length > 1)
            {
                month = splitter[0];
                year = splitter[1];
                everyYear = year == "1";
            }

            return mCM == currentMonthYear
                || (month == currentMonthYear.Split(" ")[0] && everyYear)
                || (mCM.Contains("All") && everyYear)
                || (mCM.Contains("All") && year == DateTime.Now.Year.ToString());
        }

        public static bool CheckMonthYear(int mon, int year)
        {
            string currentMonthYear = MonthYear();

            return MonthYear(mon, year) == currentMonthYear || MonthYear(mon, year).Contains("All");
        }
        public static bool CheckMonthYear(Monthly mon, string currentMonthYear)
        {
            return mon.MonthYear() == currentMonthYear || mon.MonthYear().Contains("All");
        }
        private static double GetTotalExpenses(string person = "")
        {
            double total = 0;
            foreach (var smc in StaticMonthlyCosts)
            {
                if (person == Constants.Person1)
                    total += smc.Person1Amount;
                else if (person == Constants.Person2)
                    total += smc.Person2Amount;
                else
                    total += smc.TotalAmount;
            }

            foreach (var dci in DynamicCostItems)
            {
                if (person == Constants.Person1)
                    total += dci.Person1Amount;
                else if (person == Constants.Person2)
                    total += dci.Person2Amount;
                else
                    total += dci.Amount;
            }

            return total;
        }
        public static List<LinePlot> GetPlots(int TotalNumMonths = 6)
        {
            double prevNW = Constants.Person1NetWorth + Constants.Person2NetWorth;
            SavingsGoal Default1 = Constants.SavingsGoals.FirstOrDefault(sg => sg.Person.Contains(Constants.Person1) && sg.Month == "All");
            SavingsGoal Default2 = Constants.SavingsGoals.FirstOrDefault(sg => sg.Person.Contains(Constants.Person2) && sg.Month == "All");
            List<LinePlot> plots = new List<LinePlot>();

            DateTime nextOne;
            for (int cnt = 0; cnt < TotalNumMonths; cnt++)
            {
                nextOne = DateTime.Today.AddMonths(cnt + 1);
                SetMonthlies(nextOne.Month, nextOne.Year);
                prevNW += Default1.Goal + Default2.Goal - GetTotalExpenses();
                plots.Add(new LinePlot(Constants.TheMonthsFromInt[nextOne.Month], prevNW, false));
            }

            return plots;
        }
    }
}
