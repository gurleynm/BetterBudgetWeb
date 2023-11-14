using BetterBudgetWeb.Data;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class TransactionRepo
    {
        private static HttpClient client = new HttpClient();

        private static string baseURI = Constants.BaseUri + "Transaction?token=" + Constants.Token;
        public static List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public static async Task<List<Transaction>> GetTransactionsAsync(string start = "3")
        {
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            var response = await client.GetAsync(baseURI + "&duration=" + start);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var m = response.ReasonPhrase;
                throw new ApplicationException(content);
            }
            List<Transaction> trans = System.Text.Json.JsonSerializer.Deserialize<List<Transaction>>(content, _options);
            
            Transactions = trans;
            Constants.Transactions = trans;

            return trans;
        }
        public static List<Transaction> GetTransactions()
        {
            Task.Run(async () => await GetTransactionsAsync());
            return Transactions;
        }
        // THIS IS UNIQUE!!! IT RETURNS A CatchAll!
        public static async Task<List<Transaction>> AddOrUpdateAsync(Transaction trans)
        {
            trans.Name = trans.Name.Trim();

            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, baseURI);
            
            trans.PassKey = Constants.SHA256(trans.Name + Constants.PassKey);

            requestMessage.Content = JsonContent.Create(trans);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            CatchAll ca = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);
            var tran = ca.Transactions;

            Constants.AssignCatches(ca);

            return tran;
        }
        // THIS IS UNIQUE!!! IT RETURNS A CatchAll!
        public static async Task<List<Transaction>> RemoveAsync(Transaction trans)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);
            
            trans.PassKey = Constants.SHA256(trans.Name + Constants.PassKey);

            requestMessage.Content = JsonContent.Create(trans);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            CatchAll ca = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);
            var tran = ca.Transactions;

            Constants.AssignCatches(ca);
            return tran;
        }
        // THIS IS UNIQUE!!! IT RETURNS A CatchAll!
        public static async Task<List<Transaction>> RemoveAsync(string id)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            var trans = Transactions.FirstOrDefault(t => t.Id == id);
            trans.PassKey = Constants.SHA256(trans.Name + Constants.PassKey);

            requestMessage.Content = JsonContent.Create(trans);

            var response = await _client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            CatchAll ca = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);
            var tran = ca.Transactions;
            
            Constants.AssignCatches(ca);
            return tran;
        }
    }
}
