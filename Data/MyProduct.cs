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
        public bool CurrentPlan { get; set; }
        public bool Unavailable { get; set; }
        public string SubscribeLink { get; set; }
        public MyProduct() { }
        public MyProduct(Product prod, double cost, string time, bool currentPlan = false)
        {
            if (prod == null)
                return;

            Name = prod.Name;
            Cost = cost;
            Timeframe = time;
            Description = prod.Description;

            Features.Clear();

            foreach (var feature in prod.MarketingFeatures)
                Features.Add(feature.Name);

            CurrentPlan = currentPlan;
            string RelatedSub = Name.Contains("Basic") ? "cbb_base_plan" : "cbb_advanced_plan";

            if (time == "YEAR")
                RelatedSub += "_year";
                
            SubscribeLink = $"/subscribe?plan={RelatedSub}&token={Constants.Token}";
        }
    }
}
