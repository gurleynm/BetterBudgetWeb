using BetterBudgetWeb.Data;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class PresetRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI = Constants.BaseUri + "Preset";
        public static List<Preset> Presets { get; set; } = new List<Preset>();
        public static async Task<List<Preset>> GetPresetsAsync()
        {
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = await client.GetAsync(baseURI);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            List<Preset> press = System.Text.Json.JsonSerializer.Deserialize<List<Preset>>(content, _options);

            Presets = press;
            Constants.Presets = press;

            return press;
        }
        public static List<Preset> GetPresets()
        {
            Task.Run(async () => await GetPresetsAsync());
            return Presets;
        }
        public static async Task<List<Preset>> AddOrUpdateAsync(Preset press)
        {
            press.Name = press.Name.Trim();

            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, baseURI);

            press.PassKey = Constants.PassKey;

            requestMessage.Content = JsonContent.Create(press);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var pres = JsonConvert.DeserializeObject<Preset[]>(content).ToList();
            Constants.Presets = pres;

            return pres;
        }
        public static async Task<List<Preset>> RemoveAsync(Preset press)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            press.PassKey = Constants.PassKey;

            requestMessage.Content = JsonContent.Create(press);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var pres = JsonConvert.DeserializeObject<Preset[]>(content).ToList();
            Constants.Presets = pres;
            return pres;
        }
        public static async Task<List<Preset>> RemoveAsync(string id)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            var press = Presets.FirstOrDefault(t => t.Id == id);
            press.PassKey = Constants.PassKey;

            requestMessage.Content = JsonContent.Create(press);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var pres = JsonConvert.DeserializeObject<Preset[]>(content).ToList();
            Constants.Presets = pres;
            return pres;
        }
    }
}
