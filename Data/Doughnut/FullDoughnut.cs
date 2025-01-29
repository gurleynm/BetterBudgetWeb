using BlazorBootstrap;

namespace BetterBudgetWeb.Data.Doughnut
{
    public class FullDoughnut
    {
        public DoughnutSlice this[string index]
        {
            get { return Slices[index]; }
        }
        public SortedDictionary<string, DoughnutSlice> Slices { get; set; } = new();
        public void Add(ExpenseAmount expense)
        {
            string TheKey = expense.ExpenseType;

            if (Slices.ContainsKey(TheKey))
                Slices[TheKey].Add(expense);
            else
                Slices[TheKey] = new DoughnutSlice(expense);
        }
        public List<string> GetLabels()
        {
            List<string> labels = new List<string>();

            foreach (string key in Slices.Keys)
            {
                if (Slices[key].Sum() > 0)
                    labels.Add(key);
            }

            return labels;
        }
        public List<string> GetBackgroundColors()
        {
            List<string> colors = new List<string>();

            foreach (string key in Slices.Keys)
            {
                if (Slices[key].Sum() > 0)
                    colors.Add(Slices[key].BackgroundColor);
            }

            return colors;
        }
        public List<double> GetExpenseTotals()
        {
            List<double> data = new List<double>();
            foreach (string key in Slices.Keys)
            {
                double summed = Slices[key].Sum();
                if (summed > 0)
                    data.Add(summed);
            }

            return data;
        }

        public List<ExpenseAmount> GetOrderedExpenses()
        {
            List<ExpenseAmount> OrderedExpenses = new List<ExpenseAmount>();

            foreach (string key in Slices.Keys)
            {
                OrderedExpenses.Add(new ExpenseAmount
                {
                    Amount = Slices[key].Sum(),
                    Name = key,
                    ExpenseType = key,
                    SegmentColor = Slices[key].BackgroundColor
                });
            }

            OrderedExpenses.Sort();

            return OrderedExpenses;
        }

        public void Clear()
        {
            Slices = new();
        }
    }
}
