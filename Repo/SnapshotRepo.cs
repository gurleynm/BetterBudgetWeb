using BetterBudgetWeb.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using static BetterBudgetWeb.Shared.Reports;

namespace BetterBudgetWeb.Repo
{
    public class SnapshotRepo
    {
        private static string baseURI => Constants.BaseUri + "Snapshot";
        public static List<Snapshot> Snapshots { get; set; } = new List<Snapshot>();
        public static async Task<List<Snapshot>> GetSnapshotsAsync()
        {
            if (Constants.Token == "DEMO")
                return Constants.catchAll.Snapshots;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = await client.GetAsync(baseURI);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            CatchAll catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content, _options);

            Snapshots = catcher.Snapshots;
            return Snapshots;
        }
        public static void SetDemoSnap()
        {
            if (Constants.catchAll.Snapshots == null)
                return;

            if (Constants.catchAll.Snapshots.Count(s => s.Month == DateTime.Now.ToString("MMMM") && s.Year == DateTime.Now.Year) > 0)
                return;

            Snapshot snapshot = new Snapshot();
            var TheseTrans = Constants.catchAll.Transactions.Where(t => t.MonthYear() == Constants.MonthYear()).ToList();
            var Person1T = TheseTrans.Where(t => t.Person1Amount != 0);
            var Person2T = TheseTrans.Where(t => t.Person2Amount != 0);
            snapshot.Person1Transactions = Person1T.Count();
            snapshot.Person2Transactions = Person2T.Count();
            snapshot.TotalTransactions = snapshot.Person1Transactions + snapshot.Person2Transactions;

            snapshot.Person1Income = Person1T.Sum(p => p.ExpenseType == "Income" ? p.Person1Amount : 0);
            snapshot.Person2Income = Person2T.Sum(p => p.ExpenseType == "Income" ? p.Person2Amount : 0);

            snapshot.Person1AmountActual = Person1T.Sum(p => "Income Equity Debt Transfer".Contains(p.ExpenseType) ? 0 : p.Person1Amount);
            snapshot.Person2AmountActual = Person2T.Sum(p => "Income Equity Debt Transfer".Contains(p.ExpenseType) ? 0 : p.Person2Amount);

            snapshot.Person1NetWorth = Constants.Person1NetWorth;
            snapshot.Person2NetWorth = Constants.Person2NetWorth;

            snapshot.Person1AmountProjected = Constants.DynamicCostItems.Sum(d => d.Person1Amount) + Constants.StaticMonthlyCosts.Sum(d => d.Person1Amount);
            snapshot.Person2AmountProjected = Constants.DynamicCostItems.Sum(d => d.Person2Amount) + Constants.StaticMonthlyCosts.Sum(d => d.Person2Amount);

            foreach (var d in Constants.DynamicCostItems)
            {
                double[] dciSums = SumForDynamic(TheseTrans, d.Name);
                d.SpentReport = dciSums[0] + dciSums[1];
            }

            Dynamo TodayDyno = new Dynamo
            {
                Month = snapshot.Month,
                Year = snapshot.Year.ToString(),
                DynamicCosts = new JArray()
            };

            foreach (var dci in Constants.DynamicCostItems)
            {
                dynamic jsonObject = new JObject();
                jsonObject.Name = dci.Name;
                jsonObject.Projected = dci.Person1Amount + dci.Person2Amount;
                jsonObject.Actual = dci.SpentReport;

                TodayDyno.DynamicCosts.Add(jsonObject);
            }
            var test = (string)JsonConvert.SerializeObject(TodayDyno);
            snapshot.DynamicJSON = test;
            Constants.catchAll.Snapshots.Add(snapshot);
        }
        public static double[] SumForDynamic(List<Transaction> Trans, string Expense)
        {
            double[] sums = new double[] { 0, 0, 0 };

            foreach (Transaction t in Trans)
            {
                if (t.ExpenseType == Expense && t.MonthYear() == Constants.MonthYear())
                {
                    sums[0] += t.Person1Amount;
                    sums[1] += t.Person2Amount;
                    sums[2] += t.TotalAmount;
                }
            }

            return sums;
        }
    }
}
