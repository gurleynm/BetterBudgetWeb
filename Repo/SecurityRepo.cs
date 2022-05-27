using BetterBudgetWeb.Data;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class SecurityRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI = Constants.BaseUri + "Security";
        public static List<Security> Securities { get; set; }
        public static async Task<List<Security>> GetSecuritiesAsync()
        {
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = await client.GetAsync(baseURI);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var secs = System.Text.Json.JsonSerializer.Deserialize<List<Security>>(content, _options);
            Securities = secs;

            return secs;
        }
        public static List<Security> GetSecurities()
        {
            Task.Run(async () => await GetSecuritiesAsync());
            return Securities;
        }
        public static Security GetSecurityFromName(string name)
        {
            bool FirstLoad = false;
            if (Securities == null)
            {
                Securities = GetSecurities();
                if (Securities == null) return null;

                FirstLoad = false;
            }

            var TheBal = Securities.FirstOrDefault(b => b.Name == name);

            if (TheBal == null && !FirstLoad)
            {
                Securities = GetSecurities();
                TheBal = Securities.FirstOrDefault(b => b.Name == name);
            }

            return TheBal;
        }
        public static string GetId(string name)
        {
            bool FirstLoad = false;
            if (Securities == null)
            {
                Securities = GetSecurities();
                if (Securities == null) return null;

                FirstLoad = false;
            }

            var TheBal = Securities.FirstOrDefault(b => b.Name == name);

            if (TheBal == null && !FirstLoad)
            {
                Securities = GetSecurities();
                TheBal = Securities.FirstOrDefault(b => b.Name == name);
            }

            return TheBal == null ? null : TheBal.Id;
        }
        public static string GetName(string id)
        {
            bool FirstLoad = false;
            if (Securities == null)
            {
                Securities = GetSecurities();
                if (Securities == null) return "";

                FirstLoad = false;
            }

            var TheBal = Securities.FirstOrDefault(b => b.Id == id);

            if (TheBal == null && !FirstLoad)
            {
                Securities = GetSecurities();
                TheBal = Securities.FirstOrDefault(b => b.Id == id);
            }

            return TheBal == null ? "" : TheBal.Name;
        }
        public static async Task<List<Security>> AddOrUpdateAsync(Security bal)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, baseURI);
            requestMessage.Content = JsonContent.Create(bal);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var Bals = JsonConvert.DeserializeObject<Security[]>(content).ToList();
            return Bals;
        }
        public static async Task<List<Security>> RemoveAsync(Security bal)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);
            requestMessage.Content = JsonContent.Create(bal);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var Bals = JsonConvert.DeserializeObject<Security[]>(content).ToList();
            Securities = Bals;
            return Bals;
        }
        public static List<Security> AddOrUpdate(Security bal)
        {
            Task.Run(async () => await AddOrUpdateAsync(bal));
            return Securities;
        }
        public static List<Security> Remove(Security bal)
        {
            Task.Run(async () => await RemoveAsync(bal));
            return Securities;
        }
        public static List<Security> Remove(string id)
        {
            Task.Run(async () => await RemoveAsync(Securities.FirstOrDefault(b => b.Id == id)));
            return Securities;
        }
    }
}
