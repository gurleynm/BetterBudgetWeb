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
        public TokenHolder Token { get; set; }
        public CatchAll() { }
        public CatchAll(CatchAll other)
        {
            Transactions = other.Transactions == null ? new List<Transaction>() : new List<Transaction>(other.Transactions);
            Balances = other.Balances == null ? new List<Balance>() : new List<Balance>(other.Balances);
            Monthlies = other.Monthlies == null ? new List<Monthly>() : new List<Monthly>(other.Monthlies);
            Envelopes = other.Envelopes == null ? new List<Envelope>() : new List<Envelope>(other.Envelopes);
            Presets = other.Presets == null ? new List<Preset>() : new List<Preset>(other.Presets);
            Snapshots = other.Snapshots == null ? new List<Snapshot>() : new List<Snapshot>(other.Snapshots);
            Securities = other.Securities == null ? new List<Security>() : new List<Security>(other.Securities);
            DR = new DateRange(other.DR);
        }
    }
}
