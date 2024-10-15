using BetterBudgetWeb.Data;
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
        public static async Task<List<Security>> GetSecuritiesAsync()
        {
            if (Constants.Token == "DEMO")
                return Constants.Securities;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = await client.GetAsync(string.Format(baseURI, "BLACKAPPELA", "STOCK"));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            CatchAll catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content, _options);

            Securities = new List<Security>(catcher.Securities);
            return Securities;
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

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, otherURI);

            requestMessage.Content = JsonContent.Create(small);

            var response = await client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            CatchAll catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);
            var secs = catcher.Securities;
            Constants.Securities = new List<Security>(secs);

            Securities = secs;

            return secs;
        }
        public static async Task<List<Security>> RemoveAsync(Security small)
        {
            if (Constants.Token == "DEMO")
            {
                Constants.catchAll.Securities.Remove(small);
                Constants.Securities = new List<Security>(Constants.catchAll.Securities);
                return Constants.Securities;
            }

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, otherURI);

            requestMessage.Content = JsonContent.Create(small);

            var response = await client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            CatchAll catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);

            var secs = catcher.Securities;
            Constants.Securities = new List<Security>(secs);

            Securities = secs;

            return secs;
        }
    }
}