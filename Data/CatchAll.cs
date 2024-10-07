namespace BetterBudgetWeb.Data
{
    public class CatchAll
    {
        public List<Transaction> Transactions { get; set; } = new();
        public List<Balance> Balances { get; set; } = new();
        public List<Monthly> Monthlies { get; set; } = new();
        public List<Envelope> Envelopes { get; set; } = new();
        public List<Preset> Presets { get; set; } = new();
        public List<Snapshot> Snapshots { get; set; } = new();
        public List<Security> Securities { get; set; } = new();
        public DateRange DR { get; set; } = new();
        public TokenHolder Token { get; set; } = new();
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
