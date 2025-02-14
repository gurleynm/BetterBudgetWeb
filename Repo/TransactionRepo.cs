using BetterBudgetWeb.Data;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class TransactionRepo
    {
        private static string baseURI => Constants.BaseUri + "Transaction";
        public static List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public static async Task<List<Transaction>> GetTransactionsAsync(string start = "3")
        {
            if (Constants.Token == "DEMO")
                return Constants.Transactions;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);
            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            string url = baseURI + "?duration=" + start;
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var m = response.ReasonPhrase;
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            CatchAll catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content, _options);

            Transactions = new List<Transaction>(catcher.Transactions);

            return Transactions;
        }

        // THIS IS UNIQUE!!! IT RETURNS A CatchAll!
        public static async Task<List<Transaction>> AddOrUpdateAsync(Transaction trans)
        {
            if (Constants.Token == "DEMO")
            {
                var Exists = Constants.Transactions.FirstOrDefault(t => t.Id == trans.Id);

                if (Exists == null)
                {
                    double Balance1 = -1;
                    double Balance2 = -1;


                    Balance Person1PWBalance = BalanceRepo.GetBalanceFromName(trans.PaidWithPerson1);
                    Balance Person2PWBalance = BalanceRepo.GetBalanceFromName(trans.PaidWithPerson2);

                    Balance Person1POBalance = BalanceRepo.GetBalanceFromName(trans.PaidOffPerson1);
                    Balance Person2POBalance = BalanceRepo.GetBalanceFromName(trans.PaidOffPerson2);


                    string IncomePlausible = "Income Equity";
                    int IncomeMultiplier = IncomePlausible.Contains(trans.ExpenseType) ? 1 : -1;

                    if (Constants.Envelopes.Select(e => e.Name).Contains(trans.ExpenseType))
                    {
                        Envelope NewExpEnv = Constants.Envelopes.FirstOrDefault(b => b.Name == trans.ExpenseType);

                        if (NewExpEnv != null)
                        {
                            NewExpEnv.Person1Amount -= trans.Person1Amount;
                            NewExpEnv.Person2Amount -= trans.Person2Amount;
                            if (NewExpEnv.UpdateGoal == 1)
                            {
                                NewExpEnv.Goal -= trans.Person1Amount;
                                NewExpEnv.Goal -= trans.Person2Amount;
                            }
                            await EnvelopeRepo.AddOrUpdateAsync(NewExpEnv);
                        }
                    }

                    if (Person1PWBalance != null)
                    {
                        if (Person1PWBalance.BalanceType == "Loan")
                            Person1PWBalance.Value -= trans.Person1Amount * IncomeMultiplier;
                        else
                            Person1PWBalance.Value += trans.Person1Amount * IncomeMultiplier;

                        await BalanceRepo.AddOrUpdateAsync(Person1PWBalance);
                    }

                    if (Person2PWBalance != null)
                    {
                        if (Person2PWBalance.BalanceType == "Loan")
                            Person2PWBalance.Value -= trans.Person2Amount * IncomeMultiplier;
                        else
                            Person2PWBalance.Value += trans.Person2Amount * IncomeMultiplier;

                        await BalanceRepo.AddOrUpdateAsync(Person2PWBalance);
                    }

                    if (Person1POBalance != null)
                    {
                        Person1POBalance.Value -= trans.ExpenseType == "Transfer" ? -trans.Person1Amount : trans.Person1Amount;
                        await BalanceRepo.AddOrUpdateAsync(Person1POBalance);
                    }

                    if (Person2POBalance != null)
                    {
                        Person2POBalance.Value -= trans.ExpenseType == "Transfer" ? -trans.Person2Amount : trans.Person2Amount;
                        await BalanceRepo.AddOrUpdateAsync(Person2POBalance);
                    }

                    Balance1 = Person1PWBalance != null ? Person1PWBalance.Value : -1;
                    Balance2 = Person2PWBalance != null ? Person2PWBalance.Value : -1;

                }
                else
                {
                    await RemoveAsync(Exists.Id);
                    DateTime DateMade = new DateTime(trans.DateOfTransaction.Ticks);
                    trans.DateOfTransaction = DateMade;
                    return await AddOrUpdateAsync(trans);
                }

                Constants.catchAll.Transactions.Add(trans);

                Constants.Transactions = new List<Transaction>(Constants.catchAll.Transactions);
                return Constants.Transactions;
            }

            trans.Name = trans.Name.Trim();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, baseURI);

            requestMessage.Content = JsonContent.Create(trans);

            var response = await client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            CatchAll catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);
            var tran = catcher.Transactions;


            Constants.AssignCatches(catcher);

            return tran;
        }
        
        // THIS IS UNIQUE!!! IT RETURNS A CatchAll!
        public static async Task<List<Transaction>> AddOrUpdateBatchAsync(Transaction[] MultiTrans)
        {
            if (Constants.Token == "DEMO")
            {
                List<Transaction> FullT = new();
                foreach (var trans in MultiTrans)
                    FullT = await AddOrUpdateAsync(trans);

                return FullT;
            }

            foreach (var trans in MultiTrans)
                trans.Name = trans.Name.Trim();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, baseURI + "/Batch");

            requestMessage.Content = JsonContent.Create(MultiTrans);

            var response = await client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            CatchAll catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);
            var tran = catcher.Transactions;


            Constants.AssignCatches(catcher);

            return tran;
        }

        // THIS IS UNIQUE!!! IT RETURNS A CatchAll!
        public static async Task<List<Transaction>> RemoveAsync(string id)
        {
            if (Constants.Token == "DEMO")
            {
                var Exists = Constants.catchAll.Transactions.FirstOrDefault(t => t.Id == id);
                await DemoAccountForRemove(Exists);
                Constants.catchAll.Transactions.Remove(Exists);
                Constants.Transactions = new List<Transaction>(Constants.catchAll.Transactions);

                return Constants.Transactions;
            }

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);

            var trans = Transactions.FirstOrDefault(t => t.Id == id);

            requestMessage.Content = JsonContent.Create(trans);

            var response = await client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            CatchAll catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);
            var tran = catcher.Transactions;

            Constants.AssignCatches(catcher);
            return tran;
        }

        // NEEDED FOR DEMO
        private static async Task DemoAccountForRemove(Transaction trans)
        {
            string IncomePlausible = "Income Equity";

            Balance Person1PWBalance = BalanceRepo.GetBalanceFromName(trans.PaidWithPerson1);
            Balance Person2PWBalance = BalanceRepo.GetBalanceFromName(trans.PaidWithPerson2);

            Balance Person1POBalance = BalanceRepo.GetBalanceFromName(trans.PaidOffPerson1);
            Balance Person2POBalance = BalanceRepo.GetBalanceFromName(trans.PaidOffPerson2);

            if (trans.ExpenseType == "Envelope")
            {
                Envelope Person1PWEnv = Constants.Envelopes.FirstOrDefault(b => b.Name == trans.PaidWithPerson1);
                Envelope Person2PWEnv = Constants.Envelopes.FirstOrDefault(b => b.Name == trans.PaidWithPerson2);

                if (Person1PWEnv != null)
                {
                    Person1PWBalance = Constants.Balances.FirstOrDefault(b => b.Id == Person1PWEnv.Person1Account);
                    Person1PWEnv.Person1Amount += trans.Person1Amount;
                    Person1PWEnv.Goal += Person1PWEnv.UpdateGoal == 1 ? trans.Person1Amount : 0;
                    await EnvelopeRepo.AddOrUpdateAsync(Person1PWEnv);
                }
                if (Person2PWEnv != null)
                {
                    Person2PWBalance = Constants.Balances.FirstOrDefault(b => b.Id == Person2PWEnv.Person2Account);
                    Person2PWEnv.Person2Amount += trans.Person2Amount;
                    Person2PWEnv.Goal += Person2PWEnv.UpdateGoal == 1 ? trans.Person2Amount : 0;
                    await EnvelopeRepo.AddOrUpdateAsync(Person2PWEnv);
                }
            }
            else if (Constants.Envelopes.Select(e => e.Name).Contains(trans.ExpenseType))
            {
                Envelope NewExpEnv = Constants.Envelopes.FirstOrDefault(b => b.Name == trans.ExpenseType);

                if (NewExpEnv != null)
                {
                    NewExpEnv.Person1Amount += trans.Person1Amount;
                    NewExpEnv.Person2Amount += trans.Person2Amount;
                    if (NewExpEnv.UpdateGoal == 1)
                    {
                        NewExpEnv.Goal += trans.Person1Amount;
                        NewExpEnv.Goal += trans.Person2Amount;
                    }
                    await EnvelopeRepo.AddOrUpdateAsync(NewExpEnv);
                }
            }

            int IncomeMultiplier = IncomePlausible.Contains(trans.ExpenseType) ? -1 : 1;

            if (Person1PWBalance != null)
            {
                if (Person1PWBalance.BalanceType == "Loan")
                    Person1PWBalance.Value -= trans.Person1Amount * IncomeMultiplier;
                else
                    Person1PWBalance.Value += trans.Person1Amount * IncomeMultiplier;
            }

            if (Person2PWBalance != null)
            {
                if (Person2PWBalance.BalanceType == "Loan")
                    Person2PWBalance.Value -= trans.Person2Amount * IncomeMultiplier;
                else
                    Person2PWBalance.Value += trans.Person2Amount * IncomeMultiplier;
            }

            if (Person1POBalance != null)
                Person1POBalance.Value += trans.ExpenseType == "Transfer" ? -trans.Person1Amount : trans.Person1Amount;

            if (Person2POBalance != null)
                Person2POBalance.Value += trans.ExpenseType == "Transfer" ? -trans.Person2Amount : trans.Person2Amount;

            await BalanceRepo.AddOrUpdateAsync(Person1PWBalance);
            await BalanceRepo.AddOrUpdateAsync(Person2PWBalance);

            await BalanceRepo.AddOrUpdateAsync(Person1POBalance);
            await BalanceRepo.AddOrUpdateAsync(Person2POBalance);
        }
    }
}
