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

        private static string baseURI = Constants.BaseUri + "Monthly?token=" + Constants.Token;
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
            List<Monthly> mons = System.Text.Json.JsonSerializer.Deserialize<List<Monthly>>(content, _options);

            Monthlies = mons;
            Constants.SetMonthlies(mons);
            return mons;
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

            month.PassKey = Constants.SHA256(month.Name + Constants.PassKey);

            requestMessage.Content = JsonContent.Create(month);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var tran = JsonConvert.DeserializeObject<Monthly[]>(content).ToList();
            Constants.SetMonthlies(tran);
            return tran;
        }
        public static async Task<List<Monthly>> RemoveAsync(Monthly trans)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            trans.PassKey = Constants.SHA256(trans.Name + Constants.PassKey);

            requestMessage.Content = JsonContent.Create(trans);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var tran = JsonConvert.DeserializeObject<Monthly[]>(content).ToList();
            Constants.SetMonthlies(tran);
            return tran;
        }
        public static async Task<List<Monthly>> AddOrUpdateAsync(string name, double nathanAmount, double lindseyAmount, string dyna, string monYear)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, baseURI);

            Monthly month = new Monthly(name, nathanAmount, lindseyAmount, dyna, monYear);

            month.PassKey = Constants.SHA256(month.Name + Constants.PassKey);

            requestMessage.Content = JsonContent.Create(month);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var tran = JsonConvert.DeserializeObject<Monthly[]>(content).ToList();
            Constants.SetMonthlies(tran);
            return tran;
        }
        public static async Task<List<Monthly>> RemoveAsync(string id)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            var trans = Monthlies.FirstOrDefault(t => t.Id == id);
            trans.PassKey = Constants.SHA256(trans.Name + Constants.PassKey);

            requestMessage.Content = JsonContent.Create(trans);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var tran = JsonConvert.DeserializeObject<Monthly[]>(content).ToList();
            Constants.SetMonthlies(tran);
            return tran;
        }
    }
}
