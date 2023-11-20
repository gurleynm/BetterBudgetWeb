using BetterBudgetWeb.Repo;

namespace BetterBudgetWeb.Data
{
    public class TokenWrapper
    {
        public CatchAll catcher { get; set; } = new CatchAll();
        public TokenHolder Token { get; set; } = new TokenHolder();
        public TokenWrapper() { }
    }
}
