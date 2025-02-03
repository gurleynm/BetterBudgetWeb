using BetterBudgetWeb.Data;

namespace BetterBudgetWeb.Runner
{
    public class IndexRunner
    {
        public static List<Transaction> Order(ref string DateHeaderTxt, ref string NameHeaderTxt,
                                    ref string Person1HeaderTxt, ref string Person2HeaderTxt, ref string TotalHeaderTxt,
                                    List<Transaction> FilteredTransactions, string orderBy)
        {
            switch (orderBy)
            {
                case "Date":
                    if (DateHeaderTxt == "Date")
                    {
                        FilteredTransactions = FilteredTransactions.OrderBy(ft => ft.DateOfTransaction).ToList();
                        DateHeaderTxt = "Date ↓";
                    }
                    else
                    {
                        FilteredTransactions = FilteredTransactions.OrderByDescending(ft => ft.DateOfTransaction).ToList();
                        DateHeaderTxt = "Date";
                    }

                    break;

                case "Name":
                    if (NameHeaderTxt == "Name")
                    {
                        FilteredTransactions = FilteredTransactions.OrderBy(ft => ft.Name).ToList();
                        NameHeaderTxt = "Name ↓";
                    }
                    else if (NameHeaderTxt == "Name ↓")
                    {
                        FilteredTransactions = FilteredTransactions.OrderByDescending(ft => ft.Name).ToList();
                        NameHeaderTxt = "Name ↑";
                    }
                    else
                    {
                        FilteredTransactions = FilteredTransactions.OrderByDescending(ft => ft.DateOfTransaction).ToList();
                        NameHeaderTxt = "Name";
                    }

                    break;

                case "Person1":
                    string testUp = Constants.Person1 + " ↑";
                    string testDown = Constants.Person1 + " ↓";

                    if (Person1HeaderTxt == Constants.Person1)
                    {
                        FilteredTransactions = FilteredTransactions.OrderBy(ft => ft.Person1Amount).ToList();
                        Person1HeaderTxt = testDown;
                    }
                    else if (Person1HeaderTxt == testDown)
                    {
                        FilteredTransactions = FilteredTransactions.OrderByDescending(ft => ft.Person1Amount).ToList();
                        Person1HeaderTxt = testUp;
                    }
                    else
                    {
                        FilteredTransactions = FilteredTransactions.OrderByDescending(ft => ft.DateOfTransaction).ToList();
                        Person1HeaderTxt = Constants.Person1;
                    }

                    break;

                case "Person2":
                    string test2Up = Constants.Person2 + " ↑";
                    string test2Down = Constants.Person2 + " ↓";
                    if (Person2HeaderTxt == Constants.Person2)
                    {
                        FilteredTransactions = FilteredTransactions.OrderBy(ft => ft.Person2Amount).ToList();
                        Person2HeaderTxt = test2Down;
                    }
                    else if (Person2HeaderTxt == test2Down)
                    {
                        FilteredTransactions = FilteredTransactions.OrderByDescending(ft => ft.Person2Amount).ToList();
                        Person2HeaderTxt = test2Up;
                    }
                    else
                    {
                        FilteredTransactions = FilteredTransactions.OrderByDescending(ft => ft.DateOfTransaction).ToList();
                        Person2HeaderTxt = Constants.Person2;
                    }

                    break;

                case "Total":
                    if (TotalHeaderTxt == "Total")
                    {
                        FilteredTransactions = FilteredTransactions.OrderBy(ft => ft.TotalAmount).ToList();
                        TotalHeaderTxt = "Total ↓";
                    }
                    else if (TotalHeaderTxt == "Total ↓")
                    {
                        FilteredTransactions = FilteredTransactions.OrderByDescending(ft => ft.TotalAmount).ToList();
                        TotalHeaderTxt = "Total ↑";
                    }
                    else
                    {
                        FilteredTransactions = FilteredTransactions.OrderByDescending(ft => ft.DateOfTransaction).ToList();
                        TotalHeaderTxt = "Total";
                    }

                    break;
            }

            return FilteredTransactions;
        }
        public static string TranColor(string et, double amount)
        {
            if (amount == 0) return Constants.ColorScheme["Text"];

            switch (et)
            {
                case "Income":
                    if (amount > 0)
                        return "income-good";
                    else
                        return "income-bad";
                case "Equity":
                    if (amount > 0)
                        return "equity-good";
                    else
                        return "equity-bad";
                case "Debt":
                case "Transfer":
                    return "debt";
                default:
                    if (amount > 0)
                        return "income-bad";
                    else
                        return "income-good";
            }
        }

        public static Transaction Add(NewTransaction newTrans)
        {

            string np1pw = newTrans.TopPaidWith == null ? "" : newTrans.TopPaidWith;
            string np2pw = newTrans.BottomPaidWith == null ? "" : newTrans.BottomPaidWith;

            string newExp = newTrans.ExpenseType;

            Envelope env1 = Constants.Envelopes.FirstOrDefault(e => e.Name == np1pw);
            Envelope env2 = Constants.Envelopes.FirstOrDefault(e => e.Name == np2pw);
            Envelope NewExpEnv = Constants.Envelopes.FirstOrDefault(e => e.Name == newExp);

            Transaction nt = new Transaction(newTrans.Name, newTrans.ExpenseType,
                                                newTrans.TopAmount, newTrans.BottomAmount,
                                                np1pw, np2pw, "none", "none");
            nt.DateOfTransaction = newTrans.DateOfTransaction;
            return nt;
        }
        public static double CalculateNetWorth(string person, bool UpdatePlots = true)
        {
            if (Constants.Balances == null || Constants.Balances.Count == 0)
                return 0;

            double positive = Constants.Balances.Where(bal => bal.Person == person && !bal.BalanceType.Contains("Loan") && !bal.BalanceType.Contains("Debt")).Sum(b => b.Value);
            double negative = Constants.Balances.Where(bal => bal.Person == person && bal.BalanceType.Contains("Loan")).Sum(b => b.Value);
            double joint_neg = Constants.Balances.Where(bal => bal.Person.ToUpper() == "JOINT" && bal.BalanceType.Contains("Loan")).Sum(b => b.Value) / 2;
            double joint_pos = Constants.Balances.Where(bal => bal.Person.ToUpper() == "JOINT" && !bal.BalanceType.Contains("Loan") && !bal.BalanceType.Contains("Debt")).Sum(b => b.Value) / 2;

            double stocks = Constants.Securities.Where(stock => stock.Person.ToUpper() == "JOINT").Sum(s => s.Value) / 2;
            stocks += Constants.Securities.Where(stock => stock.Person == person).Sum(s => s.Value);

            return positive + joint_pos - joint_neg - negative + stocks;
        }

        public static List<string> GetMonths(List<Transaction> trans, bool includeYear = true)
        {
            List<string> months = new List<string>();

            foreach (Transaction transaction in trans)
            {
                months.Add(transaction.DateOfTransaction.ToString("MMMM"));
            }

            List<string> ret_months = new List<string>(months);

            if (!includeYear)
            {
                ret_months.Clear();
                foreach (string month in months)
                {
                    var split = month.Split(' ');
                    if (!ret_months.Contains(split[0]))
                    {
                        ret_months.Add(split[0]);
                    }
                }
            }

            return ret_months;
        }

        public static List<string> GetMonths()
        {
            List<string> months = Constants.DR.UniqueMonthYears;
            List<string> ret_months = new List<string>(months);

            ret_months.Clear();
            foreach (string month in months)
            {
                var split = month.Split(' ');
                if (!ret_months.Contains(split[0]))
                {
                    ret_months.Add(split[0]);
                }
            }

            ret_months.Sort(Constants.SortMonths);

            return ret_months;
        }
        public static List<string> GetMonthsAndYears(List<Transaction> transactions)
        {
            List<string> uniqueMonthYears = new List<string>();

            string monthYear;
            foreach (var trans in transactions.OrderByDescending(t => t.DateOfTransaction))
            {
                monthYear = trans.DateOfTransaction.ToString("MMMM") + " " + trans.DateOfTransaction.Year.ToString();

                if (!uniqueMonthYears.Contains(monthYear))
                    uniqueMonthYears.Add(monthYear);
            }

            uniqueMonthYears.Sort(Constants.SortMonths);

            return uniqueMonthYears;
        }

        public static List<string> GetYears()
        {
            List<string> years = Constants.DR.UniqueMonthYears;
            List<string> ret_years = new List<string>(years);

            ret_years.Clear();
            foreach (string month in years)
            {
                var split = month.Split(' ');
                if (!ret_years.Contains(split[1]))
                {
                    ret_years.Add(split[1]);
                }
            }

            return ret_years;
        }
        public static List<Transaction>[] Filter(ref DynamicCostItem filter, List<Transaction> Transactions,
                                                    List<Transaction> FilteredTransactions, ref bool filtered)
        {
            List<Transaction>[] catcher = new List<Transaction>[2];

            string FilterIndicatorTxt = " (ON)";
            for (int index = 0; index < Constants.DynamicCostItems.Count; index++)
            {
                if (Constants.DynamicCostItems[index].Name.Contains(FilterIndicatorTxt) && Constants.DynamicCostItems[index] != filter)
                {
                    Constants.DynamicCostItems[index].Name = Constants.DynamicCostItems[index].Name.Replace(FilterIndicatorTxt, "");
                    ResetFilters(ref Transactions, ref FilteredTransactions, ref filtered);
                }
            }

            if (filter.Name.Contains(FilterIndicatorTxt))
            {
                ResetFilters(ref Transactions, ref FilteredTransactions, ref filtered);
                filter.Name = filter.Name.Replace(FilterIndicatorTxt, "");
                filtered = false;
            }
            else
            {
                string name = filter.Name;
                FilteredTransactions = FilteredTransactions.Where(ft => ft.ExpenseType == name).ToList();
                filter.Name += FilterIndicatorTxt;
                filtered = true;
            }
            catcher[0] = Transactions;
            catcher[1] = FilteredTransactions;

            return catcher;
        }
        private static void ResetFilters(ref List<Transaction> Transactions, ref List<Transaction> FilteredTransactions, ref bool filtered)
        {
            FilteredTransactions = Transactions.Where(t => t.MonthYear() == Constants.MonthYear()).OrderByDescending(t => t.DateOfTransaction).ToList();
            filtered = false;
        }

        public static List<LinePlot> GeneratePlots()
        {
            List<LinePlot> Plots = new();

            Snapshot snapshot;
            var Snapshots = new List<Snapshot>(Constants.catchAll.Snapshots);
            int SnapCnt = Snapshots.Count;

            DateTime CurMonthYear = DateTime.Now;
            DateTime PrevMonthYear = DateTime.Now;
            if (Constants.TIER_LEVEL == Tier.DEMO)
            {
                Snapshots.Add(new Snapshot
                {
                    Month = DateTime.Now.ToString("MMMM"),
                    Person1NetWorth = CalculateNetWorth(Constants.Person1, false),
                    Person2NetWorth = CalculateNetWorth(Constants.Person2, false)
                });
                SnapCnt++;
            }

            DateTime now = DateTime.Now;
            int MonthIndex;
            for (int index = SnapCnt - 1; Plots.Count < 6 && index > -1; index--)
            {
                snapshot = Snapshots[index];
                MonthIndex = PrevMonthYear.Month - 1;
                CurMonthYear = new DateTime(snapshot.Year, Constants.Months.IndexOf(snapshot.Month) + 1, 1);
                int Months = ((PrevMonthYear.Year - CurMonthYear.Year) * 12) + PrevMonthYear.Month - CurMonthYear.Month;

                if (Months == 0 && CurMonthYear == new DateTime(now.Year, now.Month, 1))
                    Plots.Add(new LinePlot(Constants.Months[MonthIndex], snapshot.Person1NetWorth + snapshot.Person2NetWorth, true));
                else
                    while (Months > 0)
                    {
                        MonthIndex--;
                        if (MonthIndex == -1)
                            MonthIndex = 11;
                        Months--;
                        Plots.Add(new LinePlot(Constants.Months[MonthIndex], snapshot.Person1NetWorth + snapshot.Person2NetWorth, true));
                        if (Plots.Count == 6)
                            break;
                    }

                PrevMonthYear = new DateTime(CurMonthYear.Ticks);
            }

            var existsPlot = Plots.FirstOrDefault(p => p.Month == Constants.Months[DateTime.Now.Month - 1]);

            if (Plots.Count > 0)
                Plots[0].Amount = Constants.TotalNetWorth;

            Plots.Reverse();
            return Plots;
        }
    }
}
