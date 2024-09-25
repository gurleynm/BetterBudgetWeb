using BetterBudgetWeb.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class UserRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI => Constants.BaseUri + "User";
        private static string baseTokenURI => Constants.BaseUri + "User/Token";
        private static string baseFullUserURI => Constants.BaseUri + "User/FullUser";
        public static async Task<bool> VerifyUserAsync(string user, string pass)
        {
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            var response = await client.GetAsync(baseURI + "?user=" + user + "&pass=" + pass);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return false;

            if(string.IsNullOrEmpty(content))
                return false;
            JObject j = (JObject)JsonConvert.DeserializeObject(content);

            try
            {
                string token = j["Token"].ToString();
                if (string.IsNullOrEmpty(token))
                    return false;

                Constants.Token = token;
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
        public static async Task<bool> VerifyUserAsync(string token)
        {
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            var response = await client.GetAsync(baseTokenURI + "?token=" + token);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return false;

            if(string.IsNullOrEmpty(content))
                return false;
            JObject j = (JObject)JsonConvert.DeserializeObject(content);

            try
            {
                token = j["Token"].ToString();
                if (string.IsNullOrEmpty(token))
                    return false;

                Constants.Token = token;
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
        public static async Task<User> GetFullUserAsync()
        {
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var response = await client.GetAsync(baseFullUserURI + "?token=" + Constants.Token);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return null;

            if (string.IsNullOrEmpty(content))
                return null;

            User us = JsonConvert.DeserializeObject<User>(content);

            return us;
        }
        public static async Task<bool> AddUser(string user, string user2, string email, string email2, string pass, string pass2)
        {
            if (Constants.Token == "DEMO")
                return true;

            User[] Users = new User[] { new User(user,email,pass),
                                        new User(user2,email2,pass2) };

            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            string adjustedUrl = baseURI;

            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, adjustedUrl);

            requestMessage.Content = JsonContent.Create(Users);

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

            if (content.Contains("message"))
                return false;

            CatchAll CA = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content, _options);

            Constants.Person1 = CA.Token.Name;
            Constants.Token = CA.Token.Token;

            return true;
        }
        public static async Task<string> UpdateUser(User UpdatedUser)
        {
            if (Constants.Token == "DEMO")
                return "Success";

            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            string adjustedUrl = baseURI + $"?token={Constants.Token}";

            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Put, adjustedUrl);

            requestMessage.Content = JsonContent.Create(UpdatedUser);

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

            CatchAll CA = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content, _options);

            Constants.Person1 = CA.Token.Name;
            Constants.Token = CA.Token.Token;

            return "Success";
        }
        public static async Task<bool> DeleteUser(string user, string user2, string pass)
        {
            if (Constants.Token == "DEMO")
                return true;

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

            CatchAll CA = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content, _options);

            Constants.Person1 = CA.Token.Name;
            Constants.Token = CA.Token.Token;

            return true;
        }
        public static async Task<bool> SendResetEmail(string email)
        {
            if (Constants.Token == "DEMO")
                return true;

            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            string adjustedUrl = Constants.BaseUri + "Password" + $"?email={email}";

            HttpClient _client = new HttpClient();

            var response = await client.GetAsync(adjustedUrl);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (!response.IsSuccessStatusCode)
                return false;

            if (string.IsNullOrEmpty(content))
                return false;

            return content.Contains("Success");
        }
        public static async Task<bool> VerifyResetToken(string token)
        {
            if (Constants.Token == "DEMO")
                return true;

            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            string adjustedUrl = Constants.BaseUri + "Password/" + $"?resetToken={token}";

            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Put, adjustedUrl);

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

            return content.Contains("Success");
        }
        public static async Task<bool> ResetPassword(string token, string pass)
        {
            if (Constants.Token == "DEMO")
                return true;

            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            string adjustedUrl = Constants.BaseUri + "Password/" + $"?resetToken={token}&newPass={pass}";

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
                return false;

            if (string.IsNullOrEmpty(content))
                return false;
            try
            {
                CatchAll CA = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content, _options);

                Constants.Person1 = CA.Token.Name;
                Constants.Token = CA.Token.Token;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
