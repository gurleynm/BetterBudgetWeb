using System.Globalization;

namespace BetterBudgetWeb.Data
{
    public class Preset
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
        public string HexColor { get; set; }
        public string TextColor { get; set; }
        public string PassKey { get; set; } = "no";
        public double TotalAmount => Person1Amount + Person2Amount;

        public Preset() { }
        public Preset(string name, string expenseType, double person1, double person2 = 0)
        {
            Name = name;
            ExpenseType = expenseType;
            Person1Amount = person1;
            Person2Amount = person2;
        }
        public Preset(string name, string expenseType, double person1, double person2, string person1PaidWith, string person2PaidWith)
        {
            Name = name;
            ExpenseType = expenseType;
            Person1Amount = person1;
            Person2Amount = person2;
            PaidWithPerson1 = person1PaidWith;
            PaidWithPerson2 = person2PaidWith;
        }
        public Preset(string name, string expenseType, double person1, double person2, string person1PaidWith, string person2PaidWith, string person1PaidOff, string person2PaidOff, string hex, string text)
        {
            Name = name;
            ExpenseType = expenseType;
            Person1Amount = person1;
            Person2Amount = person2;
            PaidWithPerson1 = person1PaidWith;
            PaidWithPerson2 = person2PaidWith;
            PaidOffPerson1 = person1PaidOff;
            PaidOffPerson2 = person2PaidOff;
            HexColor = hex;
            TextColor = text;
        }
        public Preset(Preset other)
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
            HexColor = other.HexColor;
            TextColor = other.TextColor;
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
                Month = ",,,,All,Presets\n\n";
                Month += ",,,," + split[0] + "," + split[1] + "\n";
            }
            else
            {
                return "no";
            }
            return Month + "Name," +
                   "Expense," +
                   "Person1 Amount," +
                   "Person2 Amount," +
                   "Total Amount," +
                   "Person1 Paid With," +
                   "Person2 Paid With," +
                   "Person1 Paid Off," +
                   "Person2 Paid Off\n";
        }
        public override string ToString()
        {
            string pwn = (string.IsNullOrEmpty(PaidWithPerson1) || PaidWithPerson1 == "none") ? "-----------------" : PaidWithPerson1;
            string pwl = string.IsNullOrEmpty(PaidWithPerson2) || PaidWithPerson2 == "none" ? "-----------------" : PaidWithPerson2;

            string pon = (string.IsNullOrEmpty(PaidOffPerson1) || PaidOffPerson1 == "none") ? "----------------" : PaidOffPerson1;
            string pol = string.IsNullOrEmpty(PaidOffPerson2) || PaidOffPerson2 == "none" ? "----------------" : PaidOffPerson2;

            int IncomeMultiplier = ExpenseType == "Income" || ExpenseType == "Debt" ? 1 : -1;

            string na = Pretty(IncomeMultiplier * Person1Amount).Replace(",", ""); ;
            string la = Pretty(IncomeMultiplier * Person2Amount).Replace(",", ""); ;

            return ExpenseType + "," +
                   Name + "," +
                   na + "," +
                   la + "," +
                   TotalAmount + "," +
                   pwn + "," +
                   pwl + "," +
                   pon + "," +
                   pol + "\n";
        }
        public static string Pretty(double num)
        {
            return num.ToString("C", CultureInfo.CurrentCulture);
        }
    }
}
