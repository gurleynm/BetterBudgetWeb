using BetterBudgetWeb.Data;
using BetterBudgetWeb.Shared;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class SecurityRepo
    {
        private static string baseURI => Constants.BaseUri + "Security?ticker={0}&SecType={1}";
        public static string otherURI => Constants.BaseUri + "Security";
        public static List<Security> Securities { get; set; } = new List<Security>();
        public static async Task<List<Security>> CallAPI(string method, string URI = "", Security small = null)
        {
            if (string.IsNullOrEmpty(URI))
                URI = baseURI;

            string content = await APIHandler.PingAPI(URI, method, small);

            if (content == null)
                return new List<Security>();

            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            CatchAll catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content, _options);

            Securities = new List<Security>(catcher.Securities);
            return Securities;
        }
        public static async Task<List<Security>> GetSecuritiesAsync()
        {
            if (Constants.Token == "DEMO")
                return Constants.Securities;

            return await CallAPI("GET", string.Format(baseURI, "BLACKAPPELA", "STOCK"));
        }
        public static List<Security> GetSecurities()
        {
            if (Constants.Token == "DEMO")
                return Constants.Securities;

            Task.Run(async () => await GetSecuritiesAsync());
            if (Securities == null)
                Securities = new List<Security>();
            return Securities;
        }
        public static async Task<List<Security>> AddOrUpdateAsync(Security small)
        {
            if (Constants.Token == "DEMO")
            {
                var Exists = Constants.Securities.FirstOrDefault(t => t.Id == small.Id);

                if (Exists == null)
                    Constants.catchAll.Securities.Add(small);
                else
                {
                    Constants.catchAll.Securities.Remove(Exists);
                    Constants.catchAll.Securities.Add(small);
                }

                Constants.Securities = new List<Security>(Constants.catchAll.Securities);
                return Constants.Securities;
            }

            return await CallAPI("POST", otherURI, small);
        }
        public static async Task<List<Security>> RemoveAsync(Security small)
        {
            if (Constants.Token == "DEMO")
            {
                Constants.catchAll.Securities.Remove(small);
                Constants.Securities = new List<Security>(Constants.catchAll.Securities);
                return Constants.Securities;
            }

            return await CallAPI("DELETE", otherURI, small);
        }
    }
}