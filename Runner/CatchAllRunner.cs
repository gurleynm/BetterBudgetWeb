using BetterBudgetWeb.Data;
using System.Text.Json;

namespace BetterBudgetWeb.Runner
{
    public class CatchAllRunner
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI = Constants.BaseUri + "CatchAll?token=" + Constants.Token;
        public static async Task<CatchAll> Grab()
        {
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = await client.GetAsync(baseURI);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            CatchAll ca = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content, _options);

            return ca;
        }
    }
}
