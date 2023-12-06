using BetterBudgetWeb.Data;
using Blazored.SessionStorage.StorageOptions;
using Newtonsoft.Json;
using System;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class SecurityRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI = Constants.BaseUri + "Security?token=" + Constants.Token + "&ticker={0}&SecType={1}";
        public static string delURI => baseURI.Substring(0, baseURI.IndexOf("?")) + "?token=" + Constants.Token;
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

            if (string.IsNullOrEmpty(content))
                return null;

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content, _options);

            Securities = new List<Security>(TW.catcher.Securities);
            return Securities;
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
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, delURI);

            requestMessage.Content = JsonContent.Create(small);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content);
            var secs = TW.catcher.Securities;

            Constants.AssignCatches(TW.catcher);
            Securities = secs;

            return secs;
        }
        public static async Task<List<Security>> RemoveAsync(Security small)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, delURI);

            requestMessage.Content = JsonContent.Create(small);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content);
            var secs = TW.catcher.Securities;

            Constants.AssignCatches(TW.catcher);
            Securities = secs;

            return secs;
        }
        public static async Task<List<Security>> RemoveAsync(string id)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, delURI);

            var small = Securities.FirstOrDefault(t => t.Id == id);

            requestMessage.Content = JsonContent.Create(small);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;


            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content);
            var secs = TW.catcher.Securities;

            Constants.AssignCatches(TW.catcher);
            Securities = secs;

            return secs;
        }
    }
}