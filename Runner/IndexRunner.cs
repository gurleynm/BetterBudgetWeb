using BetterBudgetWeb.Data;
using BetterBudgetWeb.Repo;

namespace BetterBudgetWeb.Runner
{
    public class IndexRunner
    {
        public static void Order(ref string ExpenseHeaderTxt, ref string NameHeaderTxt, 
                                    ref string Person1HeaderTxt, ref string Person2HeaderTxt, ref string TotalHeaderTxt,
                                    ref List<Transaction> FilteredTransactions, string orderBy)
        {
            switch (orderBy)
            {
                case "Expense":
                    if (ExpenseHeaderTxt == "Expense")
                    {
                        FilteredTransactions = FilteredTransactions.OrderBy(ft => ft.ExpenseType).ToList();
                        ExpenseHeaderTxt = "Expense ↑";
                    }
                    else if (ExpenseHeaderTxt == "Expense ↑")
                    {
                        FilteredTransactions = FilteredTransactions.OrderByDescending(ft => ft.ExpenseType).ToList();
                        ExpenseHeaderTxt = "Expense ↓";
                    }
                    else
                    {
                        FilteredTransactions = FilteredTransactions.OrderByDescending(ft => ft.DateOfTransaction).ToList();
                        ExpenseHeaderTxt = "Expense";
                    }

                    break;

                case "Name":
                    if (NameHeaderTxt == "Name")
                    {
                        FilteredTransactions = FilteredTransactions.OrderBy(ft => ft.Name).ToList();
                        NameHeaderTxt = "Name ↑";
                    }
                    else if (NameHeaderTxt == "Name ↑")
                    {
                        FilteredTransactions = FilteredTransactions.OrderByDescending(ft => ft.Name).ToList();
                        NameHeaderTxt = "Name ↓";
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
                        Person1HeaderTxt = testUp;
                    }
                    else if (Person1HeaderTxt == testUp)
                    {
                        FilteredTransactions = FilteredTransactions.OrderByDescending(ft => ft.Person1Amount).ToList();
                        Person1HeaderTxt = testDown;
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
                        Person2HeaderTxt = test2Up;
                    }
                    else if (Person2HeaderTxt == test2Up)
                    {
                        FilteredTransactions = FilteredTransactions.OrderByDescending(ft => ft.Person2Amount).ToList();
                        Person2HeaderTxt = test2Down;
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
                        TotalHeaderTxt = "Total ↑";
                    }
                    else if (TotalHeaderTxt == "Total ↑")
                    {
                        FilteredTransactions = FilteredTransactions.OrderByDescending(ft => ft.TotalAmount).ToList();
                        TotalHeaderTxt = "Total ↓";
                    }
                    else
                    {
                        FilteredTransactions = FilteredTransactions.OrderByDescending(ft => ft.DateOfTransaction).ToList();
                        TotalHeaderTxt = "Total";
                    }

                    break;

            }
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
                case "Debt":
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

            if (string.IsNullOrEmpty(ErrorMsg))
            {
                string nnpw = NewPerson1PaidWith == null ? "" : NewPerson1PaidWith;
                string nlpw = NewPerson2PaidWith == null ? "" : NewPerson2PaidWith;
                Transaction nt = new Transaction(NewName, NewExpense, NewPerson1Amount, NewPerson2Amount, nnpw, nlpw, "none", "none");
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

            double positive = Balances.Where(bal => bal.Person == person && !bal.BalanceType.Contains("Loan")).Sum(b => b.Value);
            double negative = Balances.Where(bal => bal.Person == person && bal.BalanceType.Contains("Loan")).Sum(b => b.Value);
            return positive - negative;
        }

        public static List<string> GetMonths(List<Transaction> Transactions)
        {
            List<string> uniqueMonthYears = new List<string>();

            if (Transactions == null)
                return uniqueMonthYears;

            string monthYear;
            foreach (var trans in Transactions.OrderByDescending(t => t.DateOfTransaction))
            {
                monthYear = trans.DateOfTransaction.ToString("MMMM") + " " + trans.DateOfTransaction.Year.ToString();

                if (!uniqueMonthYears.Contains(monthYear))
                    uniqueMonthYears.Add(monthYear);
            }

            return uniqueMonthYears;
        }
        public static void Filter(ref DynamicCostItem filter, ref List<Transaction> Transactions,
                                    ref List<Transaction> FilteredTransactions, ref bool filtered)
        {
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
        }
        private static void ResetFilters(ref List<Transaction> Transactions, ref List<Transaction> FilteredTransactions, ref bool filtered)
        {
            FilteredTransactions = Transactions.Where(t => t.MonthYear() == Constants.MonthYear()).OrderByDescending(t => t.DateOfTransaction).ToList();
            filtered = false;
        }
    }
}
