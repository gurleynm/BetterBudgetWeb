using BetterBudgetWeb.Data;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class EnvelopeRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI => Constants.BaseUri + "Envelope?token=" + Constants.Token;
        public static List<Envelope> Envelopes { get; set; } = new List<Envelope>();
        public static async Task<List<Envelope>> GetEnvelopesAsync()
        {
            if (Constants.Token == "DEMO")
                return Constants.Envelopes;

            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = await client.GetAsync(baseURI);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            CatchAll catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content, _options);
            Envelopes = catcher.Envelopes;
            return catcher.Envelopes;
        }
        public static async Task<List<Envelope>> AddOrUpdateAsync(Envelope small)
        {
            if (Constants.Token == "DEMO")
            {
                var Exists = Constants.Envelopes.FirstOrDefault(t => t.Id == small.Id);

                if (Exists == null)
                    Constants.catchAll.Envelopes.Add(small);
                else
                {
                    Constants.catchAll.Envelopes.Remove(Exists);
                    Constants.catchAll.Envelopes.Add(small);
                }

                Constants.Envelopes = new List<Envelope>(Constants.catchAll.Envelopes);
                return Constants.Envelopes;
            }
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, baseURI);

            requestMessage.Content = JsonContent.Create(small);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            CatchAll catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);
            Envelopes = catcher.Envelopes;
            return catcher.Envelopes;
        }
        public static async Task<List<Envelope>> RemoveAsync(Envelope small)
        {
            if (Constants.Token == "DEMO")
            {
                Constants.catchAll.Envelopes.Remove(small);
                Constants.Envelopes = new List<Envelope>(Constants.catchAll.Envelopes);
                return Constants.Envelopes;
            }
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            requestMessage.Content = JsonContent.Create(small);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            CatchAll catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);
            Envelopes = catcher.Envelopes;
            return catcher.Envelopes;
        }
    }
}