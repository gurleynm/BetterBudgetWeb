using BetterBudgetWeb.Data;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class MonthlyRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI => Constants.BaseUri + "Monthly?token=" + Constants.Token;
        public static List<Monthly> Monthlies { get; set; } = new List<Monthly>();
        public static async Task<List<Monthly>> GetMonthliesAsync()
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

            Monthlies = TW.catcher.Monthlies;
            Constants.SetMonthlies(TW.catcher.Monthlies);
            return TW.catcher.Monthlies;
        }
        public static List<Monthly> GetMonthlies()
        {
            Task.Run(async () => await GetMonthliesAsync());
            return Monthlies;
        }
        public static async Task<List<Monthly>> AddOrUpdateAsync(Monthly month)
        {
            month.Name = month.Name.Trim();
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, baseURI);

            requestMessage.Content = JsonContent.Create(month);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content);

            Monthlies = TW.catcher.Monthlies;
            Constants.SetMonthlies(TW.catcher.Monthlies);
            return TW.catcher.Monthlies;
        }
        public static async Task<List<Monthly>> RemoveAsync(Monthly trans)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            requestMessage.Content = JsonContent.Create(trans);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content);

            Monthlies = TW.catcher.Monthlies;
            Constants.SetMonthlies(TW.catcher.Monthlies);
            return TW.catcher.Monthlies;
        }
        public static async Task<List<Monthly>> AddOrUpdateAsync(string name, double person1Amount, double person2Amount, string dyna, string monYear)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, baseURI);

            Monthly month = new Monthly(name, person1Amount, person2Amount, dyna, monYear);

            requestMessage.Content = JsonContent.Create(month);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content);

            Monthlies = TW.catcher.Monthlies;
            Constants.SetMonthlies(TW.catcher.Monthlies);
            return TW.catcher.Monthlies;
        }
        public static async Task<List<Monthly>> RemoveAsync(string id)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            var trans = Monthlies.FirstOrDefault(t => t.Id == id);

            requestMessage.Content = JsonContent.Create(trans);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content);

            Monthlies = TW.catcher.Monthlies;
            Constants.SetMonthlies(TW.catcher.Monthlies);
            return TW.catcher.Monthlies;
        }
    }
}
