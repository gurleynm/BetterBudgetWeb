namespace BetterBudgetWeb.Data
{
    public class SplitTrans
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        private double person1Amout { get; set; }
        private double person2Amout { get; set; }
        public string ExpenseType { get; set; }
        public double Person1Amout 
        {
            get { return person1Amout; }
            set
            {
                if (person1Amout == value)
                    return;

                person1Amout = Math.Abs(value);
            }
        }
        public double Person2Amout
        {
            get { return person2Amout; }
            set
            {
                if (person2Amout == value)
                    return;

                person2Amout = Math.Abs(value);
            }
        }
        public double TotalAmount => Person1Amout + Person2Amout;
        public SplitTrans(string et)
        {
            ExpenseType = et;
        }
    }
}
