using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BetterBudgetWeb.Repo
{
    public class APIHandler
    {
        static HttpClient client = new();
        public static async Task<string> PingAPI(string uri, string callType = "", object body = null)
        {
            HttpMethod method;
            method = HttpMethod.Get;

            callType = callType.ToUpper();
            if (callType == "POST")
                method = HttpMethod.Post;
            else if (callType == "PUT")
                method = HttpMethod.Put;
            else if (callType == "DELETE")
                method = HttpMethod.Delete;

            HttpRequestMessage requestMessage = new HttpRequestMessage(method, uri);

            client.DefaultRequestHeaders.Clear();
            if (!string.IsNullOrEmpty(Constants.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);

            if (body != null)
                requestMessage.Content = JsonContent.Create(body);

            var response = await client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            if (string.IsNullOrEmpty(content))
                return null;

            if (content.Contains("Invalid token"))
            {
                Constants.TokenInvalidated = true;
                return null;
            }
            return content;
        }
    }
}
