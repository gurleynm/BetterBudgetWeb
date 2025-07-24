namespace BetterBudgetWeb.Data
{
    public class Balance
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "Savings";
        public string Person { get; set; } = Constants.Person1;
        private double val = 0;
        public double Value
        {
            get
            {
                return Math.Round(val, 2);
                //if (APR == 0)
                //    return Math.Round(val,2);
                //else
                //{
                //    int ElapsedDays = DateTime.Now.Subtract(DateUpdated).Days;
                //    double retVal = Math.Round(ElapsedDays * APR / 36500 * val + val, 2);
                //    return retVal;
                //}
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
        public double GetValueWithGoals()
        {
            double amount = Value;
            List<Data.Envelope> envs;
            int BalanceTypeModifier = BalanceType == "Loan" ? -1 : 1;

            for (int counter = 0; counter < 2; counter++)
            {
                double ChangeAmount = Constants.Envelopes
                                            .Where(en => counter == 0 ? en.Person1Account == Id :
                                                                        en.Person2Account == Id)
                                            .Sum(en => counter == 0 ? en.Person1Amount :
                                                                        en.Person2Amount);
                amount -= ChangeAmount * BalanceTypeModifier;
            }


            // Code below does ignores a Loan account type being used
            if (BalanceType != "Loan" && amount > Value)
            {
                double difference = amount - Value;
                amount = Value - difference;
            }

            amount = Math.Round(amount, 2);
            return amount;
        }

        public double GetValueWithStock()
        {
            double amount = Value;
            if (BalanceType == "Stocks")
            {
                var Securities = Constants.Securities.Where(sec => sec.BalanceFrom.ToUpper() == Id.ToUpper()).ToList();
                amount += Securities.Sum(s => s.Value);
            }

            return amount;
        }
    }
}
