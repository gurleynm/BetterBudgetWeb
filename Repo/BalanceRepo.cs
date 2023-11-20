using BetterBudgetWeb.Data;
using BetterBudgetWeb.Shared;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class BalanceRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI = Constants.BaseUri + "Balance?token=" + Constants.Token;
        public static List<Balance> Balances { get; set; }
        public static async Task<List<Balance>> GetBalancesAsync()
        {
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = await client.GetAsync(baseURI);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content, _options);
            Balances = TW.catcher.Balances;
            Constants.Balances = new List<Balance>(Balances);
            return Balances;
        }
        public static List<Balance> GetBalances()
        {
            Task.Run(async () => await GetBalancesAsync());
            return Balances;
        }
        public static Balance GetBalanceFromName(string name)
        {
            bool FirstLoad = false;
            if (Balances == null)
            {
                Balances = GetBalances();
                if (Balances == null) return null;

                FirstLoad = false;
            }

            var TheBal = Balances.FirstOrDefault(b => b.Name == name);

            if (TheBal == null && !FirstLoad)
            {
                Balances = GetBalances();
                TheBal = Balances.FirstOrDefault(b => b.Name == name);
            }

            return TheBal;
        }
        public static string GetId(string name)
        {
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

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content);
            Balances = TW.catcher.Balances;
            Constants.Balances = new List<Balance>(Balances);
            Constants.catchAll.Balances = Balances;
            return Balances;
        }
        public static async Task<List<Balance>> RemoveAsync(Balance bal)
        {
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

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content);
            Balances = TW.catcher.Balances;
            Constants.Balances = new List<Balance>(Balances);
            Constants.catchAll.Balances = Balances;
            return Balances;
        }
        public static List<Balance> AddOrUpdate(Balance bal)
        {
            Task.Run(async () => await AddOrUpdateAsync(bal));
            return Balances;
        }
        public static List<Balance> Remove(Balance bal)
        {
            Task.Run(async () => await RemoveAsync(bal));
            return Balances;
        }
        public static List<Balance> Remove(string id)
        {
            var bal = Balances.FirstOrDefault(b => b.Id == id);

            Task.Run(async () => await RemoveAsync(bal));
            return Balances;
        }
    }
}
