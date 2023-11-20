using BetterBudgetWeb.Data;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class EnvelopeRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI = Constants.BaseUri + "Envelope?token=" + Constants.Token;
        public static List<Envelope> Envelopes { get; set; } = new List<Envelope>();
        public static async Task<List<Envelope>> GetEnvelopesAsync()
        {
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = await client.GetAsync(baseURI);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content, _options);
            Envelopes = TW.catcher.Envelopes;
            return TW.catcher.Envelopes;
        }
        public static List<Envelope> GetEnvelopes()
        {
            Task.Run(async () => await GetEnvelopesAsync());
            return Envelopes;
        }
        public static async Task<List<Envelope>> AddOrUpdateAsync(Envelope small)
        {
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

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content);
            Envelopes = TW.catcher.Envelopes;
            return TW.catcher.Envelopes;
        }
        public static async Task<List<Envelope>> RemoveAsync(Envelope small)
        {
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

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content);
            Envelopes = TW.catcher.Envelopes;
            return TW.catcher.Envelopes;
        }
        public static async Task<List<Envelope>> RemoveAsync(string id)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            var small = Envelopes.FirstOrDefault(t => t.Id == id);

            requestMessage.Content = JsonContent.Create(small);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content);
            Envelopes = TW.catcher.Envelopes;
            return TW.catcher.Envelopes;
        }
    }
}