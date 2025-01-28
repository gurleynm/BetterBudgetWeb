using System.Drawing;

namespace BetterBudgetWeb.Data
{
    public class ExpenseAmount
    {

        private static Random rnd = new Random();
        public string SegmentColor { get; set; }
        public string Name { get; set; }
        public string ExpenseType { get; set; }
        public double Amount { get; set; }
        public ExpenseAmount(ExpenseAmount other) 
        { 
            Amount = other.Amount;
            SegmentColor = other.SegmentColor;
            Name = other.Name;
            ExpenseType = other.ExpenseType;
        }
        
        public ExpenseAmount(ExpenseAmount other,string color) 
        { 
            Amount = other.Amount;
            SegmentColor = color;
            Name = other.Name;
            ExpenseType = other.ExpenseType;
        }
        public ExpenseAmount(string et, double am)
        {
            Color color = ColorWheel.NextColor();
            SegmentColor = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            ExpenseType = et;
            Amount = am;
        }
        public ExpenseAmount(string et, double am, string name)
        {
            Color color = ColorWheel.NextColor();
            SegmentColor = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            ExpenseType = et;
            Amount = am;
            Name = name;
        }
        public void AddAmount(double amount)
        {
            Amount += amount;
        }
        public override string ToString()
        {
            return ExpenseType + "," + Amount + "," + SegmentColor;
        }
    }
}
