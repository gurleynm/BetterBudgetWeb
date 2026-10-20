using Stripe;

namespace BetterBudgetWeb.Data
{
    public class MyProduct
    {
        public string Name { get; set; }
        public double Cost { get; set; }
        public string Timeframe { get; set; }
        public string Description { get; set; }
        public List<string> Features { get; set; } = new List<string>();
        public Dictionary<string, double> Costs { get; set; } = new Dictionary<string, double>();
        public bool CurrentPlan { get; set; }
        public bool Unavailable { get; set; }
        public string SubscribeLink { get; set; }
        public MyProduct() { }
        public MyProduct(MyProduct other)
        {
            Name = other.Name;
            Cost = other.Cost;
            Timeframe = other.Timeframe;
            Description = other.Description;
            Features = new List<string>(other.Features);
            Costs = new Dictionary<string, double>(other.Costs);
            CurrentPlan = other.CurrentPlan;
            Unavailable = other.Unavailable;
            SubscribeLink = other.SubscribeLink;
        }
        public MyProduct(MyProduct prod, double cost, string time, bool currentPlan = false)
        {
            if (prod == null)
                return;

            Name = prod.Name;
            Cost = cost;
            Timeframe = time;
            Description = prod.Description;

            Features.Clear();

            foreach (var feature in prod.Features)
                Features.Add(feature);

            CurrentPlan = currentPlan;
            string RelatedSub = Name.Contains("Basic") ? "cbb_base_plan" : "cbb_advanced_plan";

            if (time == "YEAR")
                RelatedSub += "_year";
                
            SubscribeLink = $"/subscribe?plan={RelatedSub}&token={Constants.Token}";
        }
    }
}
