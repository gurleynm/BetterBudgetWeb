using BetterBudgetWeb.Data;
using BetterBudgetWeb.Simulation;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class SimulatedPrevRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI = Constants.BaseUri + "SimulatedPrev?id=" + Constants.Who + "&pass=" + Constants.PassKey;
        public static List<SimulatedPrev> SimulatedPrevs { get; set; } = new List<SimulatedPrev>();
        public static async Task<List<SimulatedPrev>> GetSimulatedPrevsAsync()
        {
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = await client.GetAsync(baseURI);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            List<SimulatedPrev> simp = System.Text.Json.JsonSerializer.Deserialize<List<SimulatedPrev>>(content, _options);

            SimulatedPrevs = simp;
            SimulatedConstants.SimulatedPrevs = simp;

            return simp;
        }
        public static List<SimulatedPrev> GetSimulatedPrevs()
        {
            Task.Run(async () => await GetSimulatedPrevsAsync());
            return SimulatedPrevs;
        }
        public static async Task<List<SimulatedPrev>> AddOrUpdateAsync(SimulatedPrev simp)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, baseURI);

            simp.PassKey = Constants.SHA256(simp.Month + simp.Year + Constants.PassKey);

            requestMessage.Content = JsonContent.Create(simp);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var simps = JsonConvert.DeserializeObject<SimulatedPrev[]>(content).ToList();
            SimulatedConstants.SimulatedPrevs = simps;

            return simps;
        }
        public static async Task<List<SimulatedPrev>> RemoveAsync(SimulatedPrev simp)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            simp.PassKey = Constants.SHA256(simp.Month + simp.Year + Constants.PassKey);

            requestMessage.Content = JsonContent.Create(simp);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var sim = JsonConvert.DeserializeObject<SimulatedPrev[]>(content).ToList();
            SimulatedConstants.SimulatedPrevs = sim;
            return sim;
        }
        public static async Task<List<SimulatedPrev>> RemoveAsync(string id)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            var sim = SimulatedPrevs.FirstOrDefault(t => t.Id == id);
            sim.PassKey = Constants.SHA256(sim.Month + sim.Year + Constants.PassKey);

            requestMessage.Content = JsonContent.Create(sim);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var simp = JsonConvert.DeserializeObject<SimulatedPrev[]>(content).ToList();
            SimulatedConstants.SimulatedPrevs = simp;
            return simp;
        }
    }   
}
