using BetterBudgetWeb.Repo;

namespace BetterBudgetWeb.Data
{
    public class BalanceRepoCaller
    {
        public BalanceRepoCaller() { }
        public List<Balance> GetBalances()
        {
            return BalanceRepo.GetBalances();
        }
        public Balance GetBalanceFromName(string name)
        {
            return BalanceRepo.GetBalanceFromName(name);
        }
        public string GetId(string name)
        {
            return BalanceRepo.GetId(name);
        }
        public string GetName(string id)
        {
            return BalanceRepo.GetName(id);
        }
        public List<Balance> AddOrUpdate(Balance bal)
        {
            return BalanceRepo.AddOrUpdate(bal);
        }
        public async Task<List<Balance>> AddOrUpdateAsync(Balance bal)
        {
            return await BalanceRepo.AddOrUpdateAsync(bal);
        }
        public async Task<List<Balance>> RemoveAsync(Balance bal)
        {
            return await BalanceRepo.RemoveAsync(bal);
        }
        public List<Balance> Remove(Balance bal)
        {
            return BalanceRepo.Remove(bal);
        }
        public List<Balance> Remove(string id)
        {
            return BalanceRepo.Remove(id);
        }
    }
}
