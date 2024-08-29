using BetterBudgetWeb.Data;
using BetterBudgetWeb.MonthView;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class MonthViewPrevRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI = Constants.BaseUri + "MonthViewPrev?token=" + Constants.Token;
        public static List<MonthViewPrev> MonthViewPrevs { get; set; } = new List<MonthViewPrev>();
        public static async Task<List<MonthViewPrev>> GetMonthViewPrevsAsync()
        {
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = await client.GetAsync(baseURI);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            List<MonthViewPrev> simp = System.Text.Json.JsonSerializer.Deserialize<List<MonthViewPrev>>(content, _options);

            MonthViewPrevs = simp;
            MonthViewConstants.MonthViewPrevs = simp;

            return simp;
        }
        public static List<MonthViewPrev> GetMonthViewPrevs()
        {
            Task.Run(async () => await GetMonthViewPrevsAsync());
            return MonthViewPrevs;
        }
        public static async Task<List<MonthViewPrev>> AddOrUpdateAsync(MonthViewPrev simp)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, baseURI);

            requestMessage.Content = JsonContent.Create(simp);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var simps = JsonConvert.DeserializeObject<MonthViewPrev[]>(content).ToList();
            MonthViewConstants.MonthViewPrevs = simps;

            return simps;
        }
        public static async Task<List<MonthViewPrev>> RemoveAsync(MonthViewPrev simp)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            requestMessage.Content = JsonContent.Create(simp);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var sim = JsonConvert.DeserializeObject<MonthViewPrev[]>(content).ToList();
            MonthViewConstants.MonthViewPrevs = sim;
            return sim;
        }
        public static async Task<List<MonthViewPrev>> RemoveAsync(string id)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            var sim = MonthViewPrevs.FirstOrDefault(t => t.Id == id);

            requestMessage.Content = JsonContent.Create(sim);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var simp = JsonConvert.DeserializeObject<MonthViewPrev[]>(content).ToList();
            MonthViewConstants.MonthViewPrevs = simp;
            return simp;
        }
    }   
}
