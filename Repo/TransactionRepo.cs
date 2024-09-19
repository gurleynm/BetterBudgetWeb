using BetterBudgetWeb.Data;
using BetterBudgetWeb.Runner;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class TransactionRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI => Constants.BaseUri + "Transaction?token=" + Constants.Token;
        public static List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public static async Task<List<Transaction>> GetTransactionsAsync(string start = "3")
        {
            if (Constants.Token == "DEMO")
                return Constants.Transactions;

            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var response = await client.GetAsync(baseURI + "&duration=" + start);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var m = response.ReasonPhrase;
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content, _options);

            Transactions = new List<Transaction>(TW.catcher.Transactions);

            return Transactions;
        }

        // THIS IS UNIQUE!!! IT RETURNS A CatchAll!
        public static async Task<List<Transaction>> AddOrUpdateAsync(Transaction trans)
        {
            if (Constants.Token == "DEMO")
            {
                var Exists = Constants.Transactions.FirstOrDefault(t => t.Id == trans.Id);

                if (Exists == null)
                    Constants.catchAll.Transactions.Add(trans);
                else
                {
                    Constants.catchAll.Transactions.Remove(Exists);
                    Constants.catchAll.Transactions.Add(trans);
                }

                Constants.Transactions = new List<Transaction>(Constants.catchAll.Transactions);
                return Constants.Transactions;
            }

            trans.Name = trans.Name.Trim();

            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, baseURI);

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

            var tran = TW.catcher.Transactions;


            Constants.AssignCatches(TW.catcher);

            return tran;
        }

        // THIS IS UNIQUE!!! IT RETURNS A CatchAll!
        public static async Task<List<Transaction>> RemoveAsync(string id)
        {
            if (Constants.Token == "DEMO")
            {
                var Exists = Constants.catchAll.Transactions.FirstOrDefault(t => t.Id == id);
                var c = Constants.catchAll.Transactions.Remove(Exists);
                Constants.Transactions = new List<Transaction>(Constants.catchAll.Transactions);

                return Constants.Transactions;
            }

            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            var trans = Transactions.FirstOrDefault(t => t.Id == id);

            requestMessage.Content = JsonContent.Create(trans);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            TokenWrapper TW = System.Text.Json.JsonSerializer.Deserialize<TokenWrapper>(content);
            var tran = TW.catcher.Transactions;

            Constants.AssignCatches(TW.catcher);
            return tran;
        }
    }
}
