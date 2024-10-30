using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BetterBudgetWeb.Data
{
    public class EmailService
    {
        public static async Task SendEmailAsync(string to, string subject, string body)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.gurleytech.com/Email");

            requestMessage.Content = JsonContent.Create(new EmailHelper { To = to, Subject = subject, Body = body, Key = "FranklinTheDino" });

            var response = await client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
        }
    }
}
