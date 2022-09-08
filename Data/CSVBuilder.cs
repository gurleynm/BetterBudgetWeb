namespace BetterBudgetWeb.Data
{
    public class CSVBuilder
    {
        public static string Build(List<Transaction> Transactions, string SelectedDownload)
        {
            List<Transaction> DesiredTransactions = new List<Transaction>();
            Transaction previous;
            string fileString;

            if (SelectedDownload != "All")
            {
                DesiredTransactions = Transactions.Where(tr => tr.MonthYear() == SelectedDownload).OrderByDescending(t => t.DateOfTransaction).ToList();
                previous = DesiredTransactions[0];
                fileString = Transaction.ToString(SelectedDownload);
            }
            else
            {
                DesiredTransactions = Transactions.OrderByDescending(t => t.DateOfTransaction).ToList();
                previous = DesiredTransactions[0];
                fileString = Transaction.ToString(previous.MonthYear(), true);
            }

            fileString += previous.ToString();
            Transaction current;
            double na = 0;
            double la = 0;

            for (int index = 1; index < DesiredTransactions.Count; index++)
            {
                current = new Transaction(DesiredTransactions[index]);

                if (previous.MonthYear() != current.MonthYear())
                {

                    LittleTotals(DesiredTransactions, previous, ref fileString);

                    fileString += "\n";
                    fileString += Transaction.ToString(current.MonthYear());
                }

                previous = new Transaction(current);
                fileString += current.ToString();
            }

            LittleTotals(DesiredTransactions, previous, ref fileString);

            if (SelectedDownload == "All")
            {
                na = DesiredTransactions.Where(dt => dt.ExpenseType == "Income").Sum(d => d.Person1Amount);
                la = DesiredTransactions.Where(dt => dt.ExpenseType == "Income").Sum(d => d.Person2Amount);
                double tot = na + la;
                if (na > 0 || la > 0)
                    fileString += "\nGrand Income Total:,," + Pretty(na) + "," + Pretty(la) + "," + Pretty(tot) + "\n";

                double nae = DesiredTransactions.Where(dt => dt.ExpenseType != "Income" && dt.ExpenseType != "Debt").Sum(d => d.Person1Amount);
                double lae = DesiredTransactions.Where(dt => dt.ExpenseType != "Income" && dt.ExpenseType != "Debt").Sum(d => d.Person2Amount);
                double tote = nae + lae;
                if (nae > 0 || lae > 0)
                    if (na > 0 || la > 0)
                        fileString += "Grand Expense Total:,," + Pretty(-1 * nae) + "," + Pretty(-1 * lae) + "," + Pretty(-1 * tote) + "\n";
                    else
                        fileString += "\nGrand Expense Total:,," + Pretty(-1 * nae) + "," + Pretty(-1 * lae) + "," + Pretty(-1 * tote) + "\n";

                na = na - nae;
                la = la - lae;
                tot = tot - tote;
                if (na > 0 || la > 0)
                    fileString += "Grand Net Total:,," + Pretty(na) + "," + Pretty(la) + "," + Pretty(tot) + "\n";
            }

            return fileString;
        }

        private static void LittleTotals(List<Transaction> DesiredTransactions, Transaction previous, ref string fileString)
        {
            double na = DesiredTransactions.Where(dt => dt.MonthYear() == previous.MonthYear() && dt.ExpenseType == "Income").Sum(d => d.Person1Amount);
            double la = DesiredTransactions.Where(dt => dt.MonthYear() == previous.MonthYear() && dt.ExpenseType == "Income").Sum(d => d.Person2Amount);
            double tot = na + la;
            fileString += "Income Total:,," + Pretty(na) + "," + Pretty(la) + "," + Pretty(tot) + "\n";

            double nae = DesiredTransactions.Where(dt => dt.MonthYear() == previous.MonthYear() && dt.ExpenseType != "Income" && dt.ExpenseType != "Debt").Sum(d => d.Person1Amount);
            double lae = DesiredTransactions.Where(dt => dt.MonthYear() == previous.MonthYear() && dt.ExpenseType != "Income" && dt.ExpenseType != "Debt").Sum(d => d.Person2Amount);
            double tote = nae + lae;
            fileString += "Expenses Total:,," + Pretty(-1 * nae) + "," + Pretty(-1 * lae) + "," + Pretty(-1 * tote) + "\n";

            na = na - nae;
            la = la - lae;
            tot = tot - tote;
            if (na > 0 || la > 0)
                fileString += "Net Total:,," + Pretty(na) + "," + Pretty(la) + "," + Pretty(tot) + "\n";

        }

        private static string Pretty(double num)
        {
            return Constants.Pretty(num).Replace(",", ""); ;
        }
    }
}
