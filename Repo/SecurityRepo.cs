using BetterBudgetWeb.Data;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class SecurityRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI = Constants.BaseUri + "Security?ticker={0}&SecType={1}";
        public static List<Security> Securities { get; set; } = new List<Security>();
        public static async Task<List<Security>> GetSecuritiesAsync()
        {
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = await client.GetAsync(string.Format(baseURI, "GME", "STOCK"));
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

            var smalls = JsonConvert.DeserializeObject<Security[]>(content).ToList();
            return smalls;
        }
        public static async Task<List<Security>> RemoveAsync(Security small)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            small.PassKey = Constants.SHA256(small.Name + Constants.PassKey);

            requestMessage.Content = JsonContent.Create(small);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var smalls = JsonConvert.DeserializeObject<Security[]>(content).ToList();
            return smalls;
        }
        public static async Task<List<Security>> RemoveAsync(string id)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            var small = Securities.FirstOrDefault(t => t.Id == id);
            small.PassKey = Constants.SHA256(small.Name + Constants.PassKey);

            requestMessage.Content = JsonContent.Create(small);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var smalls = JsonConvert.DeserializeObject<Security[]>(content).ToList();
            return smalls;
        }
    }
}