namespace BetterBudgetWeb.Data
{
    public class NewTransaction
    {
        public DateTime DateOfTransaction { get; set; } = DateTime.Now;
        public string Name { get; set; }
        public string ExpenseType { get; set; }

        public double TopAmount { get; set; }
        public string TopWhoPaid { get; set; }

        public double BottomAmount { get; set; }
        public string BottomWhoPaid { get; set; }

        private string topPaidWith;
        public string TopPaidWith
        {
            get { return topPaidWith; }
            set
            {
                topPaidWith = value;

                var bal = Constants.Balances.FirstOrDefault(b => b.Name == value);
                if (bal == null)
                {
                    var env = Constants.Envelopes.FirstOrDefault(e => e.Name == value);
                    if (env == null)
                        TopWhoPaid = "NONE";
                    else if (env.Person1Amount > 0 && env.Person2Amount > 0)
                        TopWhoPaid = "";
                    else if (env.Person1Amount > 0)
                        TopWhoPaid = Constants.Person1;
                    else if (env.Person2Amount > 0)
                        TopWhoPaid = Constants.Person2;
                    else
                        TopWhoPaid = "No one can pay.";
                }
                else
                {
                    if (bal.Person == Constants.Person1)
                        TopWhoPaid = Constants.Person1;
                    else if (bal.Person == Constants.Person2)
                        TopWhoPaid = Constants.Person2;
                    else
                        TopWhoPaid = "";
                }
            }
        }

        private string bottomPaidWith;
        public string BottomPaidWith
        {
            get { return bottomPaidWith; }
            set
            {
                bottomPaidWith = value;

                var bal = Constants.Balances.FirstOrDefault(b => b.Name == value);
                if (bal == null)
                {
                    var env = Constants.Envelopes.FirstOrDefault(e => e.Name == value);
                    if (env == null)
                        BottomWhoPaid = "NONE";
                    else if (env.Person1Amount > 0 && env.Person2Amount > 0)
                        BottomWhoPaid = "";
                    else if (env.Person1Amount > 0)
                        BottomWhoPaid = Constants.Person1;
                    else if (env.Person2Amount > 0)
                        BottomWhoPaid = Constants.Person2;
                    else
                        BottomWhoPaid = "No one can pay.";
                }
                else
                {
                    if (bal.Person == Constants.Person1)
                        BottomWhoPaid = Constants.Person1;
                    else if (bal.Person == Constants.Person2)
                        BottomWhoPaid = Constants.Person2;
                    else
                        BottomWhoPaid = "";
                }
            }
        }

        public double TotalAmount => Math.Round(TopAmount + BottomAmount, 2);
        public NewTransaction() { }
    }
}
