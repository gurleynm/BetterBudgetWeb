using BetterBudgetWeb.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class UserRepo
    {
        private static string baseChkURI => Constants.BaseUri + "User/CheckEmailUser";
        private static string baseURI => Constants.BaseUri + "User";
        private static string baseTokenURI => Constants.BaseUri + "User/Token";
        public static async Task<string> CallAPI(string method, string URI = "", object small = null)
        {
            if (string.IsNullOrEmpty(URI))
                URI = baseURI;

            string content = await APIHandler.PingAPI(URI, method, small);

            if (content == null)
                return null;

            return content;
        }
        public static async Task<string> CheckUserEmail(string username, string email)
        {
            string content = await CallAPI("GET", baseChkURI + "?user=" + username + "&email=" + email);

            if (string.IsNullOrEmpty(content))
                return "404";

            JObject j = (JObject)JsonConvert.DeserializeObject(content);

            try
            {
                string msg = j["message"].ToString();
                if (msg == "Success.")
                    return "";

                return j["Taken"].ToString();
            }
            catch (Exception e)
            {
                return "404";
            }
        }

        public static async Task<bool> VerifyUserAsync(string user, string pass)
        {
            string content = await CallAPI("GET", baseURI + "?user=" + user + "&pass=" + pass);

            if (string.IsNullOrEmpty(content))
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

            Constants.TokenInvalidated = false;
            return true;
        }
        public static async Task<bool> VerifyUserAsync(string token)
        {
            string CurrentToken = "" + Constants.Token;
            Constants.Token = token;
            string content = await CallAPI("GET", baseTokenURI);
            Constants.Token = CurrentToken;

            if (string.IsNullOrEmpty(content))
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

            Constants.TokenInvalidated = false;
            return true;
        }
        public static async Task<bool> ResendEmail()
        {
            if (Constants.Token == "DEMO")
                return true;

            string content = await CallAPI("POST", baseURI + $"/Resend?uid={Constants.catchAll.Token.Id}");

            if (string.IsNullOrEmpty(content))
                return false;

            Constants.Person2 = "Partner's creation email sent on " + DateTime.Now.ToString("MMMM dd, yyyy");
            return true;
        }
        public static async Task<bool> AddUser(User user)
        {
            if (Constants.Token == "DEMO")
                return true;

            string content = await CallAPI("POST","",user);

            if (string.IsNullOrEmpty(content))
                return false;

            if (content.Contains("message"))
                return false;

            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            CatchAll CA = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content, _options);

            Constants.Person1 = CA.Token.Name;
            Constants.Token = CA.Token.Token;

            return true;
        }
        public static async Task<string> UpdateUser(User UpdatedUser)
        {
            if (Constants.Token == "DEMO")
                return "Success";

            string content = await CallAPI("PUT", baseURI + $"?token={Constants.Token}", UpdatedUser);

            if (string.IsNullOrEmpty(content))
                return "No content";

            if (content.Contains("message"))
            {
                var json = JObject.Parse(content);
                return json["message"].ToString(); ;
            }

            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            CatchAll CA = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content, _options);

            Constants.Person1 = CA.Token.Name;
            Constants.Token = CA.Token.Token;

            return "Success";
        }
        public static async Task<bool> DeleteUser(string user, string user2, string pass, string pass2)
        {
            if (Constants.Token == "DEMO")
                return true;

            string content = await CallAPI("DELETE", baseURI + $"?user={user}&user2={user2}&pass={pass}&pass2={pass2}");

            if (string.IsNullOrEmpty(content))
                return false;

            if (content.Contains("message"))
                return false;

            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            CatchAll CA = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content, _options);

            Constants.Person1 = CA.Token.Name;
            Constants.Token = CA.Token.Token;

            return true;
        }
        public static async Task<bool> SendResetEmail(string email)
        {
            if (Constants.Token == "DEMO")
                return true;

            string content = await CallAPI("GET", Constants.BaseUri + "Password" + $"?email={email}");

            if (string.IsNullOrEmpty(content))
                return false;

            return content.Contains("Success");
        }
        public static async Task<string> SendCreateEmails(string email1, string email2)
        {
            if (Constants.Token == "DEMO")
                return "";

            string content = await CallAPI("POST", baseURI + "/SignUp" + $"?email={email1}&email2={email2}");

            if (string.IsNullOrEmpty(content))
                return "both";

            if (content.Contains("Success"))
                return "";

            if (content.Contains("message"))
            {
                var json = JObject.Parse(content);
                return json["message"].ToString();
            }

            return "both";
        }
        public static async Task<string> VerifyCreateToken(string token)
        {
            if (Constants.Token == "DEMO")
                return "";

            string content = await CallAPI("PUT", baseURI + "/Create" + $"?createToken={token}");

            if (string.IsNullOrEmpty(content))
                return "";

            if (content.Contains("Success"))
            {
                var json = JObject.Parse(content);
                return json["Email"].ToString();
            }

            return "";
        }

        public static async Task<bool> VerifyResetToken(string token)
        {
            if (Constants.Token == "DEMO")
                return true;

            string content = await CallAPI("PUT", Constants.BaseUri + "Password/" + $"?resetToken={token}");

            if (string.IsNullOrEmpty(content))
                return false;

            return content.Contains("Success");
        }
        public static async Task<bool> ResetPassword(string token, string pass)
        {
            if (Constants.Token == "DEMO")
                return true;

            string content = await CallAPI("POST", Constants.BaseUri + "Password/" + $"?resetToken={token}&newPass={pass}");

            if (string.IsNullOrEmpty(content))
                return false;
            try
            {
                JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
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
