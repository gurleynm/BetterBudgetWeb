using BetterBudgetWeb.Data;
using BetterBudgetWeb.Repo;

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
        public static Transaction Add(ref string ErrorMsg, ref string NewExpense, ref string NewName,
                                        ref double NewPerson1Amount, ref double NewPerson2Amount,
                                        ref string NewPerson1PaidWith, ref string NewPerson2PaidWith)
        {
            ErrorMsg = string.Empty;

            if (string.IsNullOrEmpty(NewExpense))
                ErrorMsg += "You must select an Expense type.\n";

            if (string.IsNullOrEmpty(NewName))
                ErrorMsg += "Your expense must have a Name.\n";

            if (NewPerson1Amount == 0 && NewPerson2Amount == 0)
                ErrorMsg += $"Either {Constants.Person1} or {Constants.Person2} must have a value not equal to 0.\n";

            if (NewPerson1Amount > 0 && string.IsNullOrEmpty(NewPerson1PaidWith))
                ErrorMsg += $"Must enter how {Constants.Person1}'s payment method.\n";

            if (NewPerson2Amount > 0 && string.IsNullOrEmpty(NewPerson2PaidWith))
                ErrorMsg += $"Must enter how {Constants.Person2}'s payment method.\n";

            string np1pw = NewPerson1PaidWith == null ? "" : NewPerson1PaidWith;
            string np2pw = NewPerson2PaidWith == null ? "" : NewPerson2PaidWith;

            string newExp = NewExpense;

            Envelope env1 = Constants.Envelopes.FirstOrDefault(e => e.Name == np1pw);
            Envelope env2 = Constants.Envelopes.FirstOrDefault(e => e.Name == np2pw);
            Envelope NewExpEnv = Constants.Envelopes.FirstOrDefault(e => e.Name == newExp);

            if (NewExpense == "Envelope")
            {
                if (env1 != null && NewPerson1Amount > env1.Person1Amount)
                    ErrorMsg += $"{Constants.Person1} must enter an amount ≤ {Constants.Pretty(env1.Person1Amount)} for this envelope.\n";

                if (env2 != null && NewPerson2Amount > env2.Person2Amount)
                    ErrorMsg += $"{Constants.Person2} must enter an amount ≤ {Constants.Pretty(env1.Person2Amount)} for this envelope.\n";
            }
            else if (NewExpEnv != null)
            {
                if (NewPerson1Amount > NewExpEnv.Person1Amount)
                    ErrorMsg += $"{Constants.Person1} must enter an amount ≤ {Constants.Pretty(NewExpEnv.Person1Amount)} for this envelope.\n";

                if (NewPerson2Amount > NewExpEnv.Person2Amount)
                    ErrorMsg += $"{Constants.Person2} must enter an amount ≤ {Constants.Pretty(NewExpEnv.Person2Amount)} for this envelope.\n";
            }

            if (string.IsNullOrEmpty(ErrorMsg))
            {
                Transaction nt = new Transaction(NewName, NewExpense, NewPerson1Amount, NewPerson2Amount, np1pw, np2pw, "none", "none");
                NewName = null;
                NewExpense = null;
                NewPerson1Amount = 0;
                NewPerson2Amount = 0;
                NewPerson1PaidWith = null;
                NewPerson2PaidWith = null;
                return nt;
            }

            return null;
        }
        public static double CalculateNetWorth(string person, List<Balance> Balances)
        {
            if (Balances == null)
                return 0;

            double positive = Balances.Where(bal => bal.Person == person && !bal.BalanceType.Contains("Loan") && !bal.BalanceType.Contains("Debt")).Sum(b => b.Value);
            double negative = Balances.Where(bal => bal.Person == person && bal.BalanceType.Contains("Loan")).Sum(b => b.Value);
            double joint_neg = Balances.Where(bal => bal.Person.ToUpper() == "JOINT" && bal.BalanceType.Contains("Loan")).Sum(b => b.Value) / 2;
            double joint_pos = Balances.Where(bal => bal.Person.ToUpper() == "JOINT" && !bal.BalanceType.Contains("Loan") && !bal.BalanceType.Contains("Debt")).Sum(b => b.Value) / 2;

            double stocks = 0;
            try
            {
                stocks = Constants.Securities.Where(stock => stock.Person == person || stock.Person.ToUpper() == "JOINT").Sum(s => s.Value);
            }
            catch (Exception e)
            {

            }

            return positive + joint_pos - joint_neg - negative + stocks;
        }

        public static List<string> GetMonths(List<Transaction> Transactions, bool includeYear = true)
        {
            List<string> uniqueMonthYears = new List<string>();

            if (Transactions == null)
                return uniqueMonthYears;

            string monthYear;
            foreach (var trans in Transactions.OrderByDescending(t => t.DateOfTransaction))
            {
                monthYear = includeYear ? trans.DateOfTransaction.ToString("MMMM") + " " + trans.DateOfTransaction.Year.ToString() :
                                            trans.DateOfTransaction.ToString("MMMM");

                if (!uniqueMonthYears.Contains(monthYear))
                    uniqueMonthYears.Add(monthYear);
            }

            return uniqueMonthYears;
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
    }
}
