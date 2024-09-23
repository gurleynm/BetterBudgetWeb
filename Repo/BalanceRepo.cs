using BetterBudgetWeb.Data;
using BetterBudgetWeb.Shared;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using System.Xml.Linq;

namespace BetterBudgetWeb.Repo
{
    public class BalanceRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI => Constants.BaseUri + "Balance?token=" + Constants.Token;
        public static List<Balance> Balances { get; set; }
        public static async Task<List<Balance>> GetBalancesAsync()
        {
            if(Constants.Token == "DEMO")
                return Constants.Balances;

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
            Balances = catcher.Balances;

            return Balances;
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

            bool FirstLoad = false;
            if (Balances == null)
            {
                Balances = GetBalances();
                if(Balances == null) return null;

                FirstLoad = false;
            }

            var TheBal = Balances.FirstOrDefault(b => b.Name == name);

            if (TheBal == null && !FirstLoad)
            {
                Balances = GetBalances();
                TheBal = Balances.FirstOrDefault(b => b.Name == name);
            }

            return TheBal == null ? null : TheBal.Id;
        }
        public static string GetName(string id)
        {
            if (string.IsNullOrEmpty(id))
                return "";

            bool FirstLoad = false;
            if (Balances == null)
            {
                Balances = GetBalances();
                if (Balances == null) return "";

                FirstLoad = false;
            }

            var TheBal = Balances.FirstOrDefault(b => b.Id == id);

            if (TheBal == null && !FirstLoad)
            {
                Balances = GetBalances();
                TheBal = Balances.FirstOrDefault(b => b.Id == id);
            }

            return TheBal == null ? "" : TheBal.Name;
        }
        public static async Task<List<Balance>> AddOrUpdateAsync(Balance bal)
        {
            if(bal == null)
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

            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, baseURI);

            requestMessage.Content = JsonContent.Create(bal);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            CatchAll catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);
            Balances = catcher.Balances;
            Constants.Balances = new List<Balance>(Balances);
            Constants.catchAll.Balances = Balances;
            return Balances;
        }
        public static async Task<List<Balance>> RemoveAsync(Balance bal)
        {
            if (Constants.Token == "DEMO")
            {
                Constants.catchAll.Balances.Remove(bal);
                Constants.Balances = new List<Balance>(Constants.catchAll.Balances);
                return Constants.Balances;
            }
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            requestMessage.Content = JsonContent.Create(bal);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            CatchAll catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);
            Balances = catcher.Balances;
            Constants.Balances = new List<Balance>(Balances);
            Constants.catchAll.Balances = Balances;
            return Balances;
        }
        public static Balance GetBalanceFromName(string name, bool IgnoreCase = false)
        {
            if (Balances == null)
                GetBalances();

            name = name.Replace("'", "’");

            if (IgnoreCase)
                name = name.ToUpper();

            return Balances.FirstOrDefault(b => (IgnoreCase && b.Name.ToUpper() == name) || b.Name == name);
        }
    }
}
