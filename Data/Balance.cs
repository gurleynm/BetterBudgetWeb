namespace BetterBudgetWeb.Data
{
    public class Balance
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "Savings";
        public string Person { get; set; } = "Person1";
        private double val = 0;
        public double Value 
        {
            get
            {
                if (APR == 0)
                    return Math.Round(val,2);
                else
                {
                    int ElapsedDays = DateTime.Now.Subtract(DateUpdated).Days;
                    double retVal = Math.Round(ElapsedDays * APR / 36500 * val + val, 2);
                    return retVal;
                }
            }
            set
            {
                DateUpdated = DateTime.Now;
                val = value;
            }
        }
        public string BalanceType { get; set; } = "Income";
        public double APR { get; set; } = 0;
        public string HexColor { get; set; } = "#32A852";
        public string TextColor { get; set; } = "black";
        public string PassKey { get; set; } = "no";

        public DateTime DateUpdated { get; set; } = DateTime.Now;
        public Balance() { }
        public Balance(string name, double value)
        {
            Name = name;
            Value = value;
        }
        public Balance(string name, double value, string balType, double apr, string color, string textColor, string person)
        {
            Name = name;
            Value = value;
            BalanceType = balType;
            APR = apr;
            HexColor = color;
            TextColor = textColor;
            Person = person;
        }
        public Balance(string name, double value, double apr)
        {
            Name = name;
            Value = value;
            APR = apr;
        }
        public Balance(string name, double value, string hex)
        {
            Name = name;
            Value = value;
            HexColor = hex;
        }
        public Balance(string name, double value, string hex, string person)
        {
            Name = name;
            Value = value;
            HexColor = hex;
            Person = person;
        }
        public Balance(string name, double value, double apr, string hex)
        {
            Name = name;
            Value = value;
            APR = apr;
            HexColor = hex;
        }
        public Balance(Balance other)
        {
            Id = other.Id;
            Name = other.Name;
            Value = other.Value;
            APR = other.APR;
            HexColor = other.HexColor;
        }
    }
}
