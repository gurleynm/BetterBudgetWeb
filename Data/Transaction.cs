namespace BetterBudgetWeb.Data
{
    public class Transaction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string ExpenseType { get; set; }
        public string PaidWithPerson1 { get; set; }
        public string PaidWithPerson2 { get; set; }
        public double Person1Amount { get; set; }
        public double Person2Amount { get; set; }
        public string PaidOffPerson1 { get; set; }
        public string PaidOffPerson2 { get; set; }
        public string PassKey { get; set; } = "no";
        public double TotalAmount => Person1Amount + Person2Amount;
        public DateTime DateOfTransaction { get; set; } = DateTime.Now;

        public Transaction() { }
        public Transaction(string name, string expenseType, double person1, double person2 = 0)
        {
            Name = name;
            ExpenseType = expenseType;
            Person1Amount = person1;
            Person2Amount = person2;
            DateOfTransaction = DateTime.Now;
        }
        public Transaction(string name, string expenseType, double person1, double person2, string person1PaidWith, string person2PaidWith)
        {
            Name = name;
            ExpenseType = expenseType;
            Person1Amount = person1;
            Person2Amount = person2;
            PaidWithPerson1 = person1PaidWith;
            PaidWithPerson2 = person2PaidWith;
            DateOfTransaction = DateTime.Now;
        }
        public Transaction(string name, string expenseType, double person1, double person2, string person1PaidWith, string person2PaidWith, string person1PaidOff, string person2PaidOff)
        {
            Name = name;
            ExpenseType = expenseType;
            Person1Amount = person1;
            Person2Amount = person2;
            PaidWithPerson1 = person1PaidWith;
            PaidWithPerson2 = person2PaidWith;
            PaidOffPerson1 = person1PaidOff;
            PaidOffPerson2 = person2PaidOff;
            DateOfTransaction = DateTime.Now;
        }
        public Transaction(Transaction other)
        {
            Id = other.Id;
            Name = other.Name;
            ExpenseType = other.ExpenseType;
            Person1Amount = other.Person1Amount;
            Person2Amount = other.Person2Amount;
            PaidWithPerson1 = other.PaidWithPerson1;
            PaidWithPerson2 = other.PaidWithPerson2;
            PaidOffPerson1 = other.PaidOffPerson1;
            PaidOffPerson2 = other.PaidOffPerson2;
            DateOfTransaction = other.DateOfTransaction;
        }
        public string MonthYear()
        {
            return DateOfTransaction.ToString("MMMM") + " " + DateOfTransaction.Year.ToString();
        }
        public static string ToString(string header, bool all = false)
        {
            string[] split = header.Split(" ");
            string Month;

            if (split.Length > 1 && !all)
            {
                Month = ",,,," + split[0] + "," + split[1] + "\n";
            }
            else if (split.Length > 1 && all)
            {
                Month = ",,,,All,Transactions\n\n";
                Month += ",,,," + split[0] + "," + split[1] + "\n";
            }
            else
            {
                return "no";
            }
            return Month + "Name," +
                   "Expense," +
                   $"{Constants.Person1} Amount," +
                   $"{Constants.Person2} Amount," +
                   "Total Amount," +
                   $"{Constants.Person1} Paid With," +
                   $"{Constants.Person2} Paid With," +
                   $"{Constants.Person1} Paid Off," +
                   $"{Constants.Person2} Paid Off," +
                   "Date Of Transaction\n";
        }
        public override string ToString()
        {
            string pwn = (string.IsNullOrEmpty(PaidWithPerson1) || PaidWithPerson1 == "none") ? "-----------------" : PaidWithPerson1;
            string pwl = string.IsNullOrEmpty(PaidWithPerson2) || PaidWithPerson2 == "none" ? "-----------------" : PaidWithPerson2;

            string pon = (string.IsNullOrEmpty(PaidOffPerson1) || PaidOffPerson1 == "none") ? "----------------" : PaidOffPerson1;
            string pol = string.IsNullOrEmpty(PaidOffPerson2) || PaidOffPerson2 == "none" ? "----------------" : PaidOffPerson2;

            int IncomeMultiplier = ExpenseType == "Income" || ExpenseType == "Debt" ? 1 : -1;

            string na = Constants.Pretty(IncomeMultiplier * Person1Amount).Replace(",", ""); ;
            string la = Constants.Pretty(IncomeMultiplier * Person2Amount).Replace(",", ""); ;

            return ExpenseType + "," +
                   Name + "," +
                   na + "," +
                   la + "," +
                   TotalAmount + "," +
                   pwn + "," +
                   pwl + "," +
                   pon + "," +
                   pol + "," +
                   DateOfTransaction.Date.ToShortDateString() + "\n";
        }
    }
}
