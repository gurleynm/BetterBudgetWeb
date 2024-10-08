﻿using BetterBudgetWeb.Data;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetterBudgetWeb.Repo
{
    public class MonthlyRepo
    {
        private static string baseURI => Constants.BaseUri + "Monthly";
        public static List<Monthly> Monthlies { get; set; } = new List<Monthly>();
        public static async Task<List<Monthly>> GetMonthliesAsync()
        {
            if (Constants.Token == "DEMO")
                return Constants.Monthlies;

            JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);
            var response = await client.GetAsync(baseURI);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            CatchAll catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content, _options);

            Monthlies = catcher.Monthlies;
            Constants.SetMonthlies(catcher.Monthlies);
            return catcher.Monthlies;
        }
        public static List<Monthly> GetMonthlies()
        {
            if (Constants.Token == "DEMO")
                return Constants.Monthlies;

            Task.Run(async () => await GetMonthliesAsync());
            return Monthlies;
        }
        public static async Task<List<Monthly>> RemoveAsync(Monthly trans)
        {
            if (Constants.Token == "DEMO")
            {
                Constants.catchAll.Monthlies.Remove(trans);
                Constants.Monthlies = new List<Monthly>(Constants.catchAll.Monthlies);
                return Constants.Monthlies;
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, baseURI);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);

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

            Monthlies = catcher.Monthlies;
            Constants.SetMonthlies(catcher.Monthlies);
            return catcher.Monthlies;
        }
        public static async Task<List<Monthly>> AddOrUpdateAsync(string name, double person1Amount, double person2Amount, string dyna, string monYear)
        {
            if (Constants.Token == "DEMO")
            {
                var m = new Monthly(name, person1Amount, person2Amount, dyna, monYear);
                var Exists = Constants.Monthlies.FirstOrDefault(t => t.Name == name);

                if (Exists == null)
                    Constants.catchAll.Monthlies.Add(m);
                else
                {
                    Constants.catchAll.Monthlies.Remove(Exists);
                    Constants.catchAll.Monthlies.Add(m);
                }

                Constants.Monthlies = new List<Monthly>(Constants.catchAll.Monthlies);

                return Constants.Monthlies;
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, baseURI);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);

            Monthly month = new Monthly(name, person1Amount, person2Amount, dyna, monYear);

            requestMessage.Content = JsonContent.Create(month);

            var response = await client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            if (string.IsNullOrEmpty(content))
                return null;

            CatchAll catcher = System.Text.Json.JsonSerializer.Deserialize<CatchAll>(content);

            Monthlies = catcher.Monthlies;
            Constants.SetMonthlies(catcher.Monthlies);
            return catcher.Monthlies;
        }
    }
}
