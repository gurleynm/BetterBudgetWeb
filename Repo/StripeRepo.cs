using BetterBudgetWeb.Data;
using Stripe;
using Stripe.Checkout;

namespace BetterBudgetWeb.Repo
{
    public class StripeRepo
    {
        private static string baseURI => Constants.BaseUri + "Stripe";
        private static HttpClient client;
        public static async Task<List<MyProduct>> CallAPI(string method)
        {
            string content = await APIHandler.PingAPI(baseURI, method);

            if (content == null)
                return new List<MyProduct>();

            var catcher = System.Text.Json.JsonSerializer.Deserialize<List<MyProduct>>(content);

            return catcher;
        }
        public static async Task<List<MyProduct>> GetProducts()
        {
            return await CallAPI("GET");
        }

        public static async Task<string> CheckoutUrl()
        {
            Session session = await CreateCheckoutSession(new List<string>());
            return session.Url;
        }

        public static async Task<Session> CreateCheckoutSession(List<string> items)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = "price_1Q2DWE2L4K66u9tvZEChFmS4",
                        Quantity = 1,
                    },
                },
                Mode = "subscription",
                SuccessUrl = "https://localhost:7151/success",
                CancelUrl = "https://localhost:7151/pay",
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);
            return session;
        }
    }
}
