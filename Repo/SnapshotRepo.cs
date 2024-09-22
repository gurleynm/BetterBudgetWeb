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
            if (Constants.Token == "DEMO")
                return Constants.catchAll.Snapshots;

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

            Snapshots = catcher.Snapshots;
            return Snapshots;
        }
    }
}
