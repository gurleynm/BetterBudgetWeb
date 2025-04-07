using BetterBudgetWeb.Data;
using Stripe;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class EnvelopeRepo
    {
        private static string baseURI => Constants.BaseUri + "Envelope";
        public static List<Envelope> Envelopes { get; set; } = new List<Envelope>();
        public static async Task<List<Envelope>> CallAPI(string method, Envelope small = null)
        {
            string content = await APIHandler.PingAPI(baseURI, method, small);

            if (content == null)
                return new List<Envelope>();

            var catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);
            Envelopes = new(catcher.Envelopes);
            Constants.Envelopes = new(Envelopes);
            Constants.catchAll.Envelopes = new(Envelopes);
            Constants.BalancesChanged.Invoke(null, Constants.Balances);

            return Envelopes;
        }
        public static async Task<List<Envelope>> GetEnvelopesAsync()
        {
            if (Constants.Token == "DEMO")
                return Constants.Envelopes;

            return await CallAPI("GET");
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

            return await CallAPI("POST",small);
        }
        public static async Task<List<Envelope>> RemoveAsync(Envelope small)
        {
            if (Constants.Token == "DEMO")
            {
                Constants.catchAll.Envelopes.Remove(small);
                Constants.Envelopes = new List<Envelope>(Constants.catchAll.Envelopes);
                return Constants.Envelopes;
            }

            return await CallAPI("DELETE", small);
        }
    }
}