using BlazorBootstrap;

namespace BetterBudgetWeb.Data.Doughnut
{
    public class DoughnutSlice
    {
        public DoughnutSlice() { }
        public DoughnutSlice(string name) 
        {
            Name = name;
        }
        public DoughnutSlice(ExpenseAmount expense)
        {
            Name = expense.ExpenseType;
            Add(expense);
        }
        public string Name { get; set; }
        public List<ExpenseAmount> SliceValues { get; set; } = new();
        public string BackgroundColor { get; set; }
        public void Add(ExpenseAmount expense)
        {
            if (string.IsNullOrEmpty(BackgroundColor))
                BackgroundColor = ColorWheel.NextColorString();

            SliceValues.Add(new ExpenseAmount(expense, BackgroundColor));
        }
        public double Sum()
        {
            return SliceValues.Sum(e => e.Amount);
        }
    }
}
