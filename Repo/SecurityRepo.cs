using BetterBudgetWeb.Data;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class SecurityRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI = Constants.BaseUri + "Security?token=" + Constants.Token + "&ticker={0}&SecType={1}";
        public static string delURI => baseURI.Substring(0, baseURI.IndexOf("?"));
        public static List<Security> Securities { get; set; } = new List<Security>();
        public static async Task<List<Security>> GetSecuritiesAsync()
        {
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = await client.GetAsync(string.Format(baseURI, "BLACKAPPELA", "STOCK"));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            List<Security> smalls = System.Text.Json.JsonSerializer.Deserialize<List<Security>>(content, _options);

            Securities = smalls;
            return smalls;
        }
        public static List<Security> GetSecurities()
        {
            Task.Run(async () => await GetSecuritiesAsync());
            if(Securities == null)
                Securities = new List<Security>();
            return Securities;
        }
        public static async Task<List<Security>> AddOrUpdateAsync(Security small)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, baseURI);

            small.PassKey = Constants.SHA256(small.Name + Constants.PassKey);

            requestMessage.Content = JsonContent.Create(small);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            CatchAll ca = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);
            var secs = ca.Securities;

            Constants.AssignCatches(ca);
            Securities = secs;

            return secs;
        }
        public static async Task<List<Security>> RemoveAsync(Security small)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, delURI);

            small.PassKey = Constants.SHA256(small.Name + Constants.PassKey);

            requestMessage.Content = JsonContent.Create(small);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            CatchAll ca = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);
            var secs = ca.Securities;

            Constants.AssignCatches(ca);
            Securities = secs;

            return secs;
        }
        public static async Task<List<Security>> RemoveAsync(string id)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, delURI);

            var small = Securities.FirstOrDefault(t => t.Id == id);
            small.PassKey = Constants.SHA256(small.Name + Constants.PassKey);

            requestMessage.Content = JsonContent.Create(small);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            CatchAll ca = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);
            var secs = ca.Securities;

            Constants.AssignCatches(ca);
            Securities = secs;

            return secs;
        }
    }
}