using BetterBudgetWeb.Data;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class SnapshotRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI => Constants.BaseUri + "Snapshot?token=" + Constants.Token;
        public static List<Snapshot> Snapshots { get; set; } = new List<Snapshot>();
        public static async Task<List<Snapshot>> GetSnapshotsAsync()
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

            Snapshots = TW.catcher.Snapshots;
            return Snapshots;
        }
        public static List<Snapshot> GetSnapshots()
        {
            Task.Run(async () => await GetSnapshotsAsync());
            return Snapshots;
        }
        public static async Task<List<Snapshot>> AddOrUpdateAsync(Snapshot snap)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, baseURI);

            requestMessage.Content = JsonContent.Create(snap);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content);
            var tran = TW.catcher.Snapshots;
            return tran;
        }
        public static async Task<List<Snapshot>> RemoveAsync(Snapshot trans)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            requestMessage.Content = JsonContent.Create(trans);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content);
            var tran = TW.catcher.Snapshots;
            return tran;
        }
        public static async Task<List<Snapshot>> RemoveAsync(string id)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            var snaps = Snapshots.FirstOrDefault(t => t.Id == id);

            requestMessage.Content = JsonContent.Create(snaps);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content);
            var tran = TW.catcher.Snapshots;
            return tran;
        }
    }
}
