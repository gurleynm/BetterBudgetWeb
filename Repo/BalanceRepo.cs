using BetterBudgetWeb.Data;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Xml.Linq;

namespace BetterBudgetWeb.Repo
{
    public class BalanceRepo
    {
        private static string baseURI => Constants.BaseUri + "Balance";
        public static List<Balance> Balances { get; set; }
        public static async Task<List<Balance>> CallAPI(string method, Balance small = null)
        {
            string content = await APIHandler.PingAPI(baseURI, method, small);

            if (content == null)
                return new List<Balance>();

            var catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);
            Balances = new(catcher.Balances);
            Constants.Balances = new(Balances);
            Constants.catchAll.Balances = new(Balances);

            return Balances;
        }
        public static async Task<List<Balance>> GetBalancesAsync()
        {
            if (Constants.Token == "DEMO")
                return Constants.Balances;

            return await CallAPI("GET");
        }
        public static List<Balance> GetBalances()
        {
            if (Constants.Token == "DEMO")
                return Constants.Balances;

            Task.Run(async () => await GetBalancesAsync());
            return Balances;
        }

        public static string GetId(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "";

            var TheBal = Balances?.FirstOrDefault(b => b.Name == name);
            return TheBal == null ? null : TheBal.Id;
        }
        public static string GetName(string id)
        {
            if (string.IsNullOrEmpty(id) || id == "NONE")
                return "";

            var TheBal = Constants.Balances?.FirstOrDefault(b => b.Id == id);

            return TheBal == null ? "" : TheBal.Name;
        }
        public static async Task<List<Balance>> AddOrUpdateAsync(Balance bal)
        {
            if (bal == null)
                return Constants.Balances;
            if (Constants.Token == "DEMO")
            {
                var Exists = Constants.Balances.FirstOrDefault(t => t.Id == bal.Id);

                if (Exists == null)
                    Constants.catchAll.Balances.Add(bal);
                else
                {
                    Constants.catchAll.Balances.Remove(Exists);
                    Constants.catchAll.Balances.Add(bal);
                }

                Constants.Balances = new List<Balance>(Constants.catchAll.Balances);
                return Constants.Balances;
            }

            bal.Name = bal.Name.Trim();

            return await CallAPI("POST", bal);
        }
        public static async Task<List<Balance>> RemoveAsync(Balance bal)
        {
            if (Constants.Token == "DEMO")
            {
                Constants.catchAll.Balances.Remove(bal);
                Constants.Balances = new List<Balance>(Constants.catchAll.Balances);
                return Constants.Balances;
            }

            return await CallAPI("DELETE", bal);
        }
        public static Balance GetBalanceFromName(string name, bool IgnoreCase = false)
        {
            name = name.Replace("'", "’");

            if (IgnoreCase)
                name = name.ToUpper();

            return Constants.Balances?.FirstOrDefault(b => (IgnoreCase && b.Name.ToUpper() == name) || b.Name == name);
        }
        public static string GetNameFromId(string id)
        {
            Balance chose = Constants.Balances?.FirstOrDefault(b => b.Id == id);
            if (chose == null)
                return "";

            return chose.Name;
        }
        public static (double, double) GetFromTo(Transaction TheTransaction, string balName)
        {
            var trans = new List<Transaction>(Constants.Transactions.Where(t => t.DateOfTransaction.CompareTo(TheTransaction.DateOfTransaction) >= 0
                                                                            && t.OneOfTheBalances(balName)).OrderByDescending(t => t.DateOfTransaction));
            if (Constants.Balances.FirstOrDefault(b => b.Name == balName) is Balance foundBal)
            {
                int IncomeMultiplier = foundBal.BalanceType == "Loan" ? -1 : 1;
                double CurrentVal = foundBal.Value;
                double PrevVal = CurrentVal;

                for (int index = 0; index < trans.Count; index++)
                {
                    PrevVal = CurrentVal;
                    SetChangedAmount(foundBal, trans[index], ref CurrentVal);

                    if (trans[index].Id == TheTransaction.Id)
                    {
                        return (CurrentVal, PrevVal);

                    }
                }
            }

            return (0, 0);
        }

        private static void SetChangedAmount(Balance b, Transaction t, ref double curVal)
        {
            string balName = b.Name;
            int IncomeMultiplier = 0;
            if (b.BalanceType == "Loan")
            {
                IncomeMultiplier = -Constants.IncomeMultiplier(t.ExpenseType, false);
                // Credit Cards, Loans, etc.
                if (t.PaidWithPerson1 == balName)
                {
                    // Person 1 paid with balance
                    curVal -= t.Person1Amount * IncomeMultiplier;
                }

                if (t.PaidWithPerson2 == balName)
                {
                    // Person 2 paid with balance
                    curVal -= t.Person2Amount * IncomeMultiplier;
                }

                if (t.PaidOffPerson1 == balName)
                {
                    // Person 1 paid off balance
                    curVal += t.Person1Amount * IncomeMultiplier;
                }

                if (t.PaidOffPerson2 == balName)
                {
                    // Person 2 paid off balance                
                    curVal += t.Person2Amount * IncomeMultiplier;
                }
            }
            else
            {
                IncomeMultiplier = -Constants.IncomeMultiplier(t.ExpenseType, false);

                // Equity, Savings
                if (t.PaidWithPerson1 == balName)
                {
                    // Person 1 paid with balance
                    curVal += t.Person1Amount * IncomeMultiplier;
                }

                if (t.PaidWithPerson2 == balName)
                {
                    // Person 2 paid with balance
                    curVal += t.Person2Amount * IncomeMultiplier;
                }

                if (t.PaidOffPerson1 == balName)
                {
                    // Person 1 paid off balance
                    curVal -= t.Person1Amount * IncomeMultiplier;
                }

                if (t.PaidOffPerson2 == balName)
                {
                    // Person 2 paid off balance
                    curVal -= t.Person2Amount * IncomeMultiplier;
                }
            }
        }
    }
}
