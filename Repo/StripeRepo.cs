using Stripe;
using Stripe.Checkout;

namespace BetterBudgetWeb.Repo
{
    public class StripeRepo
    {
        private static HttpClient client;
        private static string baseURI => "https://api.stripe.com/v1/products";

        public static async Task<StripeList<Product>> GetProducts()
        {
            var options = new ProductListOptions { Limit = 3 };
            var service = new ProductService();
            StripeList<Product> products = await service.ListAsync(options);

            return products;
        }
        public static async Task<StripeList<Price>> GetPrices()
        {
            var service = new PriceService();
            StripeList<Price> prices = await service.ListAsync();

            return prices;
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
