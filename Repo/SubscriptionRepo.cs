using BetterBudgetWeb.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class SubscriptionRepo
    {
        private static string baseURI => Constants.BaseUri + "Subscription?usersid=" + Constants.catchAll.Token.Id;
        public static async Task<string> CallAPI(string method, string URI = "", object small = null)
        {
            if (string.IsNullOrEmpty(URI))
                URI = baseURI;

            string content = await APIHandler.PingAPI(URI, method, small);

            if (content == null)
                return null;

            return content;
        }
        public static async Task<string> GetSubscriptionAsync()
        {
            if (Constants.Token == "DEMO")
            {
                Constants.TIER_LEVEL = Tier.GOD_TIER;
                return "GOD_TIER";
            }

            string content = await CallAPI("GET");

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


            string content = await CallAPI("GET",Constants.BaseUri + $"Subscription/Token?token={Constants.Token}");

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
