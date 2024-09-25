using BetterBudgetWeb.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class SubscriptionRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI => Constants.BaseUri + "Subscription?userid=" + Constants.catchAll.Token.Id;
        public static async Task<string> GetSubscriptionAsync()
        {
            if (Constants.Token == "DEMO")
            {
                Constants.TIER_LEVEL = Tier.GOD_TIER;
                return "GOD_TIER";
            }

            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = await client.GetAsync(baseURI);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            JObject retStr = (JObject)JsonConvert.DeserializeObject(content);
            string status = retStr["status"].ToString();
            string tier = retStr["Tier"].ToString();

            switch (tier)
            {
                case "BASIC_TIER":
                    Constants.TIER_LEVEL = Tier.BASIC_TIER;
                    break;

                case "ADVANCED_TIER":
                    Constants.TIER_LEVEL = Tier.ADVANCED_TIER;
                    break;

                case "GOD_TIER":
                    Constants.TIER_LEVEL = Tier.GOD_TIER;
                    break;
                default:
                    Constants.TIER_LEVEL = Tier.TRIAL;                    
                    break;
            }

            return tier;
        }
    }
}
