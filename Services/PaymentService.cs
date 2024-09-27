using Stripe.Checkout;

namespace BetterBudgetWeb.Services
{
    public class PaymentService : IPaymentService
    {
        public PaymentService() { }
        public Session CreateCheckoutSession(List<string> items)
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
            Session session = service.Create(options);
            return session;
        }
    }
}
