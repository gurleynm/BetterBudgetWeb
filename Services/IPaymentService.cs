using Stripe.Checkout;

namespace BetterBudgetWeb.Services
{
    public interface IPaymentService
    {
        Session CreateCheckoutSession(List<string> items);
    }
}
