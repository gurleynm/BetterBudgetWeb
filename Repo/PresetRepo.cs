using BetterBudgetWeb.Data;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class PresetRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI = Constants.BaseUri + "Preset?token=" + Constants.Token;
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

            if (string.IsNullOrEmpty(content))
                return null;

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content, _options);

            Presets = TW.catcher.Presets;
            Constants.Presets = new List<Preset>(Presets);

            return Presets;
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

            requestMessage.Content = JsonContent.Create(press);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            var TW = JsonConvert.DeserializeObject<TokenWrapper>(content);
            Constants.Presets = new List<Preset>(TW.catcher.Presets);

            return TW.catcher.Presets;
        }
        public static async Task<List<Preset>> RemoveAsync(Preset press)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            requestMessage.Content = JsonContent.Create(press);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            var TW = JsonConvert.DeserializeObject<TokenWrapper>(content);
            Constants.Presets = new List<Preset>(TW.catcher.Presets);
            return TW.catcher.Presets;
        }
        public static async Task<List<Preset>> RemoveAsync(string id)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            var press = Presets.FirstOrDefault(t => t.Id == id);

            requestMessage.Content = JsonContent.Create(press);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            var TW = JsonConvert.DeserializeObject<TokenWrapper>(content);
            Constants.Presets = new List<Preset>(TW.catcher.Presets);
            return TW.catcher.Presets;
        }
    }
}
