using Newtonsoft.Json.Linq;

namespace BetterBudgetWeb.Data
{
    public class ReportHelperClass
    {
        public class Dynamo
        {
            public string Month = "";
            public string Year = "";
            public JArray DynamicCosts;
        }

        public class MonthTab
        {
            public string MyMonth = "";
            public string MyYear = "";
            public string MyClass = "";
        }

        public class YearTab
        {
            public string MyYear = "";
            public string MyClass = "";
        }
    }
}
