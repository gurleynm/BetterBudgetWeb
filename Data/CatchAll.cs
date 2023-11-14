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
        public List<Security> Securities { get; set; }
        public DateRange DR { get; set; }
        public CatchAll() { }
        public CatchAll(CatchAll other)
        {
            Transactions = new List<Transaction>(other.Transactions);
            Balances = new List<Balance>(other.Balances);
            Monthlies = new List<Monthly>(other.Monthlies);
            Envelopes = new List<Envelope>(other.Envelopes);
            Presets = new List<Preset>(other.Presets);
            Snapshots = new List<Snapshot>(other.Snapshots);
            Securities = new List<Security>(other.Securities);
            DR = new DateRange(other.DR);
        }
    }
}
