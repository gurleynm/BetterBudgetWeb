using BetterBudgetWeb.Data;

namespace BetterBudgetWeb.Runner
{
    public class ReportRunner
    {
        public static List<Monthly> DynamicCostItems { get; set; }
        public static List<SavingsGoal> SavingsGoals { get; set; }
        public static List<Monthly> StaticMonthlyCosts { get; set; }
        public static List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public static List<Monthly> Monthlies { get; set; } = new List<Monthly>();
        public static List<Balance> Balances { get; set; } = new List<Balance>();
        public static List<ProjectedDatum> ProjectedData { get; set; }

        public static int Month { get; set; } = DateTime.Now.Month;
        public static int Year { get; set; } = DateTime.Now.Year;

        public static void SetMonthlies(int month, int year)
        {
            Monthlies = Constants.GetMonthlies();

            Month = month;
            Year = year;

            DynamicCostItems = new List<Monthly>();
            SavingsGoals = new List<SavingsGoal>();
            StaticMonthlyCosts = new List<Monthly>();
            ProjectedData = new List<ProjectedDatum>();

            foreach (var mon in Monthlies)
            {
                if (mon.Dynamic == "DYNAMIC")
                {
                    if (CheckMonthYear(mon))
                    {
                        var DCIExists = DynamicCostItems.FirstOrDefault(d => d.Name == mon.Name);
                        var NewDCI = new Monthly(mon.Name, mon.Person1Amount, mon.Person2Amount, "DYNAMIC");

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
                        var NewSMC = new Monthly(mon.Name, mon.Person1Amount, mon.Person2Amount, "STATIC");

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

            DynamicCostItems = DynamicCostItems.OrderByDescending(dci => dci.TotalAmount).ToList();
            StaticMonthlyCosts = StaticMonthlyCosts.OrderByDescending(smc => smc.TotalAmount).ToList();
            SavingsGoals = SavingsGoals.OrderByDescending(sg => sg.Goal).ToList();
        }
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
        public static string MonthYear() { return new DateTime(Year, Month, 1).ToString("MMMM") + " " + Year.ToString(); }
        public static double[] SumForDynamic(List<Transaction> Trans, string Expense)
        {
            double[] sums = new double[] { 0, 0, 0 };

            foreach (Transaction t in Trans)
            {
                if (t.ExpenseType == Expense && t.MonthYear() == MonthYear())
                {
                    sums[0] += t.Person1Amount;
                    sums[1] += t.Person2Amount;
                    sums[2] += t.TotalAmount;
                }
            }

            return sums;
        }
        public static double SumForDynamicAll(List<Transaction> Trans, int person = 0)
        {
            double sum = 0;

            foreach (Transaction t in Trans)
            {
                if (DynamicCostItems.Select(d => d.Name).Contains(t.ExpenseType) && t.MonthYear() == MonthYear())
                {
                    if (person == 1)
                        sum += t.Person1Amount;
                    else if (person == 2)
                        sum += t.Person2Amount;
                    else
                        sum += t.TotalAmount;
                }
            }

            return sum;
        }

        public static double SumForStaticAll(List<Transaction> Trans, int person = 0)
        {
            double sum = 0;

            foreach (Transaction t in Trans)
            {
                if (StaticMonthlyCosts.Select(d => d.Name).Contains(t.Name) && t.MonthYear() == MonthYear())
                {
                    if (person == 1)
                        sum += t.Person1Amount;
                    else if (person == 2)
                        sum += t.Person2Amount;
                    else
                        sum += t.TotalAmount;
                }
            }

            return sum;
        }

        public static double GetMonthExpense(List<Transaction> Trans, int person, string selMonth)
        {
            if (selMonth == "All")
                return GetAllExpense(Trans, person);

            return SumForDynamicAll(Trans, person) + SumForStaticAll(Trans, person);
        }

        public static double GetAllExpense(List<Transaction> Trans, int person = 0)
        {
            double sum = 0;

            foreach (Transaction t in Trans)
            {
                if (StaticMonthlyCosts.Select(d => d.Name).Contains(t.Name))
                {
                    if (person == 1)
                        sum += t.Person1Amount;
                    else if (person == 2)
                        sum += t.Person2Amount;
                    else
                        sum += t.TotalAmount;
                }
                else if (DynamicCostItems.Select(d => d.Name).Contains(t.ExpenseType))
                {
                    if (person == 1)
                        sum += t.Person1Amount;
                    else if (person == 2)
                        sum += t.Person2Amount;
                    else
                        sum += t.TotalAmount;
                }
            }

            return sum;
        }


        public static double GetMonthNet(List<Transaction> Trans, int person, string selMonth)
        {
            double dynam = GetMonthExpense(Trans, person, selMonth);
            double income;

            if (person == 1)
                income = Trans.Where(tr => tr.ExpenseType == "Income").Sum(t => t.Person1Amount);
            else if (person == 2)
                income = Trans.Where(tr => tr.ExpenseType == "Income").Sum(t => t.Person2Amount);
            else
                income = Trans.Where(tr => tr.ExpenseType == "Income").Sum(t => t.TotalAmount);

            return income - dynam;
        }
    }
}
