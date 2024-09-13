using BetterBudgetWeb.Data;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class UserRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI => Constants.BaseUri + "User";
        public static async Task<bool> VerifyUserAsync(string user, string pass)
        {
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            var response = await client.GetAsync(baseURI + "?user=" + user + "&pass=" + pass);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return false;

            if(string.IsNullOrEmpty(content))
                return false;

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content, _options);

            Constants.TW = TW;
            Constants.Person1 = TW.Token.Name;
            Constants.Token = TW.Token.Token;

            return true;
        }
        public static async Task<bool> VerifyUserAsync(string token)
        {
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            var response = await client.GetAsync(baseURI + "?user=" + token + "&pass=CHECK_AS_TOKEN");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return false;

            if(string.IsNullOrEmpty(content))
                return false;

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content, _options);

            Constants.TW = TW;
            Constants.Person1 = TW.Token.Name;
            Constants.Token = TW.Token.Token;

            return true;
        }
        public static async Task<bool> AddUser(string user, string user2, string pass)
        {
            return (await UpdateUser(user, user2, pass, "NONE")) == "Success";
        }
        public static async Task<string> UpdateUser(string user, string user2, string pass, string newPass)
        {
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            string adjustedUrl = baseURI + $"?user={user}&user2={user2}&pass={pass}&newPass={newPass}";

            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, adjustedUrl);

            requestMessage.Content = JsonContent.Create("");

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (!response.IsSuccessStatusCode)
                return "Invalid response";

            if (string.IsNullOrEmpty(content))
                return "No content";

            if (content.Contains("message"))
            {
                var json = JObject.Parse(content);
                return json["message"].ToString(); ;
            }

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content, _options);

            Constants.TW = TW;
            Constants.Person1 = TW.Token.Name;
            Constants.Token = TW.Token.Token;

            return "Success";
        }
        public static async Task<bool> DeleteUser(string user, string user2, string pass)
        {
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            string adjustedUrl = baseURI + $"?user={user}&user2={user2}&pass={pass}";

            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, adjustedUrl);

            requestMessage.Content = JsonContent.Create("");

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (!response.IsSuccessStatusCode)
                return false;

            if (string.IsNullOrEmpty(content))
                return false;

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content, _options);

            Constants.TW = TW;
            Constants.Person1 = TW.Token.Name;
            Constants.Token = TW.Token.Token;

            return true;
        }
    }
}
