using BetterBudgetWeb.Data;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BetterBudgetWeb.Runner
{
    public class CatchAllRunner
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI => Constants.BaseUri + "CatchAll";
        public static async Task<CatchAll> Grab()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);

            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = await client.GetAsync(baseURI);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            CatchAll CA = JsonSerializer.Deserialize<CatchAll>(content, _options);
            Constants.TIER_STATUS = CA.Token.Status;
            Constants.TIER_LEVEL = Constants.StringToTier(CA.Token.Tier);
            Constants.DeviceSubscribed = CA.Token.Device;
            Constants.CUR_USER_EMAIL = CA.Token.Email;

            return CA;
        }
        public static async Task<CatchAll> GrabDemo()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "DEMO");

            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = await client.GetAsync(baseURI);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            CatchAll CA = JsonSerializer.Deserialize<CatchAll>(content, _options);
            Constants.catchAll = CA;
            Constants.TIER_LEVEL = Tier.DEMO;
            Constants.Token = "DEMO";

            await Constants.Init(true);

            return CA;
        }
    }
}
