using BetterBudgetWeb.Data;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class PresetRepo
    {
        private static string baseURI => Constants.BaseUri + "Preset";
        public static List<Preset> Presets { get; set; } = new List<Preset>();
        public static async Task<List<Preset>> GetPresetsAsync()
        {
            if (Constants.Token == "DEMO")
                return Constants.Presets;


            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);
            var response = await client.GetAsync(baseURI);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            CatchAll catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content, _options);

            Presets = catcher.Presets;
            Constants.Presets = new List<Preset>(Presets);

            return Presets;
        }
        public static async Task<List<Preset>> AddOrUpdateAsync(Preset press)
        {
            if (Constants.Token == "DEMO")
            {
                var Exists = Constants.Presets.FirstOrDefault(t => t.Id == press.Id);

                if (Exists == null)
                    Constants.catchAll.Presets.Add(press);
                else
                {
                    Constants.catchAll.Presets.Remove(Exists);
                    Constants.catchAll.Presets.Add(press);
                }

                Constants.Presets = new List<Preset>(Constants.catchAll.Presets);
                return Constants.Presets;
            }
            press.Name = press.Name.Trim();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token); ;
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, baseURI);

            requestMessage.Content = JsonContent.Create(press);

            var response = await client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            CatchAll catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);
            Constants.Presets = new List<Preset>(catcher.Presets);

            return catcher.Presets;
        }
        public static async Task<List<Preset>> RemoveAsync(Preset press)
        {
            if (Constants.Token == "DEMO")
            {
                Constants.catchAll.Presets.Remove(press);
                Constants.Presets = new List<Preset>(Constants.catchAll.Presets);
                return Constants.Presets;
            }
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            requestMessage.Content = JsonContent.Create(press);

            var response = await client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            CatchAll catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);
            Constants.Presets = new List<Preset>(catcher.Presets);
            return catcher.Presets;
        }
    }
}
