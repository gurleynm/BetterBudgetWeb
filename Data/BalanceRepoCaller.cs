using BetterBudgetWeb.Repo;

namespace BetterBudgetWeb.Data
{
    public class BalanceRepoCaller
    {
        public BalanceRepoCaller() { }
        public string GetName(string id)
        {
            return BalanceRepo.GetName(id);
        }
        public async Task<List<Balance>> AddOrUpdateAsync(Balance bal)
        {
            return await BalanceRepo.AddOrUpdateAsync(bal);
        }
    }
}
