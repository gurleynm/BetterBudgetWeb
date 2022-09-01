using BetterBudgetWeb.Repo;

namespace BetterBudgetWeb.Data
{
    public class CatchAll
    {
        public List<Transaction> Transactions { get; set; }
        public List<Balance> Balances { get; set; }
        public List<Monthly> Monthlies { get; set; }
        public List<Envelope> Envelopes { get; set; }
        public List<Preset> Presets { get; set; }
        public List<Snapshot> Snapshots { get; set; }
    }
}
