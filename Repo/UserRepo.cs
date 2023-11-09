using BetterBudgetWeb.Data;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class UserRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI = Constants.BaseUri + "User";
        public static async Task<User> VerifyUserAsync(string user, string pass)
        {
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            var response = await client.GetAsync(baseURI + "?user=" + user + "&pass=" + pass);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return null;

            if(string.IsNullOrEmpty(content))
                return null;

            User TheUser = System.Text.Json.JsonSerializer.Deserialize<User>(content, _options);

            return TheUser;
        }
    }
}
