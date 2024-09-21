using BetterBudgetWeb.Data;
using System.Text.Json;

namespace BetterBudgetWeb.Runner
{
    public class CatchAllRunner
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI => Constants.BaseUri + "CatchAll?token=" + Constants.Token;
        public static async Task<CatchAll> Grab()
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
            return TW.catcher;
        }
        public static async Task<TokenWrapper> GrabDemo()
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

            TokenWrapper TW = JsonSerializer.Deserialize<TokenWrapper>(content, _options);

            Constants.TW = TW;
            Constants.Person1 = TW.Token.Name;
            Constants.Token = TW.Token.Token;
            Constants.catchAll = new CatchAll(TW.catcher);
            await Constants.Init(true);

            return TW;
        }
    }
}
