using BetterBudgetWeb.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class SubscriptionRepo
    {
        private static string baseURI => Constants.BaseUri + "Subscription?usersid=" + Constants.catchAll.Token.Id;
        public static async Task<string> GetSubscriptionAsync()
        {
            if (Constants.Token == "DEMO")
            {
                Constants.TIER_LEVEL = Tier.GOD_TIER;
                return "GOD_TIER";
            }

            HttpClient client = new HttpClient();

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
            string device = retStr["Device"].ToString();

            Constants.TIER_STATUS = retStr["status"].ToString();
            Constants.TIER_LEVEL = Constants.StringToTier(tier);
            Constants.DeviceSubscribed = device;

            return tier;
        }
        public static async Task<(string,string,string)> GetMobileSubscriptionAsync()
        {
            if (Constants.Token == "DEMO")
            {
                Constants.TIER_LEVEL = Tier.GOD_TIER;
                return ("active","GOD_TIER",Constants.Device);
            }

            HttpClient client = new HttpClient();

            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            string url = Constants.BaseUri + $"Subscription/Token?token={Constants.Token}";

            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return ("inactive","","");

            JObject retStr = (JObject)JsonConvert.DeserializeObject(content);

            string status = retStr["status"].ToString();
            string tier = retStr["Tier"].ToString();
            string device = retStr["Device"].ToString();

            return (status,tier,device);
        }
    }
}
