namespace BetterBudgetWeb.Data
{
    public class CSVBuilder
    {
        static string[] GoodUps = new string[] { "Income", "Equity" };
        static string[] Ignores = new string[] { "Income", "Equity", "Transfer", "Debt" };
        public static string Build(List<Transaction> Transactions, string SelectedDownload)
        {
            List<Transaction> DesiredTransactions = new List<Transaction>();
            Transaction previous;
            string fileString;


            if (SelectedDownload == "All All")
            {
                // Everything
                DesiredTransactions = Transactions.OrderByDescending(t => t.DateOfTransaction).ToList();
                previous = DesiredTransactions[0];
                fileString = Transaction.ToString(previous.MonthYear(), true);
            }
            else if (SelectedDownload.Contains(" All"))
            {
                // All Years
                var SelectedMonth = SelectedDownload.Split(' ')[0];

                DesiredTransactions = Transactions.Where(t =>
                                            t.MonthYear().Split(' ')[0] == SelectedMonth).OrderByDescending(t => t.DateOfTransaction).ToList();
                previous = DesiredTransactions[0];
                fileString = Transaction.ToString(previous.MonthYear(), true);
            }
            else if (SelectedDownload.Contains("All "))
            {
                // All Months
                var SelectedYear = SelectedDownload.Split(' ')[1];

                DesiredTransactions = Transactions.Where(t =>
                                            t.MonthYear().Split(' ')[1] == SelectedYear).OrderByDescending(t => t.DateOfTransaction).ToList();
                previous = DesiredTransactions[0];
                fileString = Transaction.ToString(previous.MonthYear(), true);
            }
            else
            {
                DesiredTransactions = Transactions.Where(tr => tr.MonthYear() == SelectedDownload).OrderByDescending(t => t.DateOfTransaction).ToList();
                previous = DesiredTransactions[0];
                fileString = Transaction.ToString(SelectedDownload);
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
                na = DesiredTransactions.Where(dt => GoodUps.Contains(dt.ExpenseType)).Sum(d => d.Person1Amount);
                la = DesiredTransactions.Where(dt => GoodUps.Contains(dt.ExpenseType)).Sum(d => d.Person2Amount);
                double tot = na + la;
                if (na > 0 || la > 0)
                    fileString += "\nGrand Income Total:,," + Pretty(na) + "," + Pretty(la) + "," + Pretty(tot) + "\n";

                double nae = DesiredTransactions.Where(dt => !Ignores.Contains(dt.ExpenseType)).Sum(d => d.Person1Amount);
                double lae = DesiredTransactions.Where(dt => !Ignores.Contains(dt.ExpenseType)).Sum(d => d.Person2Amount);
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
        public static string BuildSankey(List<Transaction> Transactions, string SelectedDownload)
        {
            List<Transaction> DesiredTransactions = new List<Transaction>();
            Dictionary<string, Transaction> DesiredTransactionsDict = new Dictionary<string, Transaction>();
            Dictionary<string, Transaction> DictIncome = new Dictionary<string, Transaction>();

            string fileString = "";

            if (SelectedDownload != "All")
                DesiredTransactions = Transactions.Where(tr => tr.MonthYear() == SelectedDownload).OrderByDescending(t => t.DateOfTransaction).ToList();
            else
                DesiredTransactions = Transactions.OrderByDescending(t => t.DateOfTransaction).ToList();

            string TKey;
            string[] IgnoreThese = new string[] { "Debt", "Equity", "Transfer", "Envelope" };

            foreach (var transact in DesiredTransactions)
            {
                if (IgnoreThese.Contains(transact.ExpenseType))
                    continue;

                TKey = transact.Name;

                if (transact.ExpenseType == "Income")
                {
                    if (DictIncome.ContainsKey(TKey))
                    {
                        DictIncome[TKey].Person1Amount += transact.Person1Amount;
                        DictIncome[TKey].Person2Amount += transact.Person2Amount;
                    }
                    else
                    {
                        DictIncome[TKey] = new Transaction(transact);
                    }
                    continue;
                }


                if (DesiredTransactionsDict.ContainsKey(TKey))
                {
                    DesiredTransactionsDict[TKey].Person1Amount += transact.Person1Amount;
                    DesiredTransactionsDict[TKey].Person2Amount += transact.Person2Amount;
                }
                else
                {
                    DesiredTransactionsDict[TKey] = new Transaction(transact);
                }
            }

            Dictionary<string, Transaction> GeneralTransactions = new Dictionary<string, Transaction>();
            Transaction trans;
            var keys = DesiredTransactionsDict.Keys.ToArray();
            Array.Sort(keys);
            foreach (var key in keys)
            {
                if (DesiredTransactionsDict[key].TotalAmount < 0)
                    continue;

                fileString += DesiredTransactionsDict[key].ToString("Sankey");

                trans = DesiredTransactionsDict[key];

                if (trans.ExpenseType == "Income")
                    continue;
                else if (trans.ExpenseType == "Envelope")
                    TKey = trans.Name;
                else if (IgnoreThese.Contains(trans.ExpenseType) || trans.ExpenseType == "Income")
                    continue;
                else
                    TKey = trans.ExpenseType;

                if (GeneralTransactions.ContainsKey(TKey))
                {
                    GeneralTransactions[TKey].Person1Amount += trans.Person1Amount;
                    GeneralTransactions[TKey].Person2Amount += trans.Person2Amount;
                }
                else
                {
                    if (trans.ExpenseType == "Envelope")
                        GeneralTransactions[TKey] = new Transaction
                        {
                            Person1Amount = trans.Person1Amount,
                            Person2Amount = trans.Person2Amount,
                            ExpenseType = trans.Name,
                            Name = trans.ExpenseType
                        };
                    else
                        GeneralTransactions[TKey] = new Transaction(trans);
                }
            }

            foreach (var key in DictIncome.Keys)
                fileString += DictIncome[key].ToString("Sankey");

            foreach (var key in GeneralTransactions.Keys)
                fileString += GeneralTransactions[key].ToString("Sankey", "GENERAL");

            return fileString;
        }

        private static void LittleTotals(List<Transaction> DesiredTransactions, Transaction previous, ref string fileString)
        {
            double na = DesiredTransactions.Where(dt => dt.MonthYear() == previous.MonthYear() && GoodUps.Contains(dt.ExpenseType)).Sum(d => d.Person1Amount);
            double la = DesiredTransactions.Where(dt => dt.MonthYear() == previous.MonthYear() && GoodUps.Contains(dt.ExpenseType)).Sum(d => d.Person2Amount);
            double tot = na + la;
            fileString += "Income Total:,," + Pretty(na) + "," + Pretty(la) + "," + Pretty(tot) + "\n";

            double nae = DesiredTransactions.Where(dt => dt.MonthYear() == previous.MonthYear() && !Ignores.Contains(dt.ExpenseType)).Sum(d => d.Person1Amount);
            double lae = DesiredTransactions.Where(dt => dt.MonthYear() == previous.MonthYear() && !Ignores.Contains(dt.ExpenseType)).Sum(d => d.Person2Amount);
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
