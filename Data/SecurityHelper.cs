using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BetterBudgetWeb.Data
{
    public class SecurityHelper
    {
        public static double Help(string type, string response)
        {
            switch (type)
            {
                case "Crypto":
                    return CryptoHelper(response); 

                case "ETF":
                    return ETFHelper(response);

                case "Call":
                case "Put":
                    return OptionHelper(response);

                default:
                    return StockHelper(response);
            }
        }
        private static double CryptoHelper(string response)
        {
            return -1;
        }
        private static double ETFHelper(string response)
        {
            return -1;
        }
        private static double OptionHelper(string response)
        {
            return -1;
        }
        private static double StockHelper(string response)
        {
            JObject j = (JObject)JsonConvert.DeserializeObject(response);
            string lastSalePrice = null;

            try
            {
                JObject data = (JObject)j["data"];
                JObject primaryData = (JObject)data["primaryData"];
                lastSalePrice = primaryData["lastSalePrice"].ToString();
            }
            catch (Exception e)
            {                
                return -1;
            }

            return Convert.ToDouble(lastSalePrice.Substring(1));
        }
    }
}
