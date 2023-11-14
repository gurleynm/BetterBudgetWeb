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

        private static string baseURI = Constants.BaseUri + "Snapshot?token=" + Constants.Token;
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
            List<Snapshot> snaps = System.Text.Json.JsonSerializer.Deserialize<List<Snapshot>>(content, _options);

            Snapshots = snaps;
            return snaps;
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

            snap.PassKey = Constants.SHA256(snap.Month + snap.Year + Constants.PassKey);

            requestMessage.Content = JsonContent.Create(snap);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var tran = JsonConvert.DeserializeObject<Snapshot[]>(content).ToList();
            return tran;
        }
        public static async Task<List<Snapshot>> RemoveAsync(Snapshot trans)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            trans.PassKey = Constants.SHA256(trans.Month + trans.Year + Constants.PassKey);

            requestMessage.Content = JsonContent.Create(trans);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var tran = JsonConvert.DeserializeObject<Snapshot[]>(content).ToList();
            return tran;
        }
        public static async Task<List<Snapshot>> RemoveAsync(string id)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            var snaps = Snapshots.FirstOrDefault(t => t.Id == id);
            snaps.PassKey = Constants.SHA256(snaps.Month + snaps.Year + Constants.PassKey);

            requestMessage.Content = JsonContent.Create(snaps);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var tran = JsonConvert.DeserializeObject<Snapshot[]>(content).ToList();
            return tran;
        }
    }
}
