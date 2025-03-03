using BetterBudgetWeb.Data;
using BetterBudgetWeb.Shared;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class PresetRepo
    {
        private static string baseURI => Constants.BaseUri + "Preset";
        public static List<Preset> Presets { get; set; } = new List<Preset>();
        public static async Task<List<Preset>> CallAPI(string method, Preset small = null)
        {
            string content = await APIHandler.PingAPI(baseURI, method, small);

            if (content == null)
                return new List<Preset>();

            var catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);

            Presets = catcher.Presets;
            Constants.Presets = new List<Preset>(Presets);

            return Presets;
        }
        public static async Task<List<Preset>> GetPresetsAsync()
        {
            if (Constants.Token == "DEMO")
                return Constants.Presets;

            return await CallAPI("GET");
        }
        public static async Task<List<Preset>> AddOrUpdateAsync(Preset press)
        {
            if (Constants.Token == "DEMO")
            {
                var Exists = Constants.Presets.FirstOrDefault(t => t.Id == press.Id);

                if (Exists == null)
                    Constants.catchAll.Presets.Add(press);
                else
                {
                    Constants.catchAll.Presets.Remove(Exists);
                    Constants.catchAll.Presets.Add(press);
                }

                Constants.Presets = new List<Preset>(Constants.catchAll.Presets);
                return Constants.Presets;
            }
            press.Name = press.Name.Trim();

            return await CallAPI("POST",press);
        }
        public static async Task<List<Preset>> RemoveAsync(Preset press)
        {
            if (Constants.Token == "DEMO")
            {
                Constants.catchAll.Presets.Remove(press);
                Constants.Presets = new List<Preset>(Constants.catchAll.Presets);
                return Constants.Presets;
            }

            return await CallAPI("DELETE", press);
        }
    }
}
