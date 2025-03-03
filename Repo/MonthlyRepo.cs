using BetterBudgetWeb.Data;
using BetterBudgetWeb.Shared;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class MonthlyRepo
    {
        private static string baseURI => Constants.BaseUri + "Monthly";
        public static List<Monthly> Monthlies { get; set; } = new List<Monthly>();
        public static async Task<List<Monthly>> CallAPI(string method, Monthly small = null)
        {
            string content = await APIHandler.PingAPI(baseURI, method, small);

            if (content == null)
                return new List<Monthly>();

            var catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);

            Monthlies = catcher.Monthlies;
            Constants.SetMonthlies(catcher.Monthlies);

            return Monthlies;
        }
        public static async Task<List<Monthly>> GetMonthliesAsync()
        {
            if (Constants.Token == "DEMO")
                return Constants.Monthlies;

            return await CallAPI("GET");
        }
        public static List<Monthly> GetMonthlies()
        {
            if (Constants.Token == "DEMO")
                return Constants.Monthlies;

            Task.Run(async () => await GetMonthliesAsync());
            return Monthlies;
        }
        public static async Task<List<Monthly>> RemoveAsync(Monthly trans)
        {
            if (Constants.Token == "DEMO")
            {
                Constants.catchAll.Monthlies.Remove(trans);
                Constants.Monthlies = new List<Monthly>(Constants.catchAll.Monthlies);
                return Constants.Monthlies;
            }

            return await CallAPI("DELETE",trans);
        }
        public static async Task<List<Monthly>> AddOrUpdateAsync(string name, double person1Amount, double person2Amount, string dyna, string monYear)
        {
            if (Constants.Token == "DEMO")
            {
                var m = new Monthly(name, person1Amount, person2Amount, dyna, monYear);
                var Exists = Constants.Monthlies.FirstOrDefault(t => t.Name == name);

                if (Exists == null)
                    Constants.catchAll.Monthlies.Add(m);
                else
                {
                    Constants.catchAll.Monthlies.Remove(Exists);
                    Constants.catchAll.Monthlies.Add(m);
                }

                Constants.Monthlies = new List<Monthly>(Constants.catchAll.Monthlies);

                return Constants.Monthlies;
            }

            Monthly month = new Monthly(name, person1Amount, person2Amount, dyna, monYear);

            return await CallAPI("POST",month);
        }
    }
}
