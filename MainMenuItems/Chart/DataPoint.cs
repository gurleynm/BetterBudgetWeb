namespace BetterBudgetWeb.MainMenuItems.Chart
{
    public class DataPoint
    {
        public string Label { get; set; }
        public double Value { get; set; }
        public bool Negative { get; set; }
        public DataPoint() { }
        public DataPoint(string label, string value)
        {
            Label = label;
            bool tp = double.TryParse(value, out double v);
            if(tp)
                Value = v;
        }
        
        public DataPoint(string label, double value)
        {
            Label = label;
            Value = value;
        }
    }
}
