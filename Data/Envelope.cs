namespace BetterBudgetWeb.Data
{
    public class Envelope
    {
        public string Id { get; set; } =  Guid.NewGuid().ToString();

        public string Name { get; set; }

        public double Goal { get; set; }

        public double Person1Amount { get; set; }

        public double Person2Amount { get; set; }

        public double TotalAmount => Person1Amount + Person2Amount;

        public string Person1Account { get; set; }

        public string Person2Account { get; set; }
        
        public int UpdateGoal { get; set; }
        
        public Envelope() { }

        public Envelope(string Id)
        {
            this.Id = Id;
        }

        public Envelope(string Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }

        public Envelope(string Id, string Name, double Goal)
        {
            this.Id = Id;
            this.Name = Name;
            this.Goal = Goal;
        }

        public Envelope(string Id, string Name, double Goal, double Person1Amount)
        {
            this.Id = Id;
            this.Name = Name;
            this.Goal = Goal;
            this.Person1Amount = Person1Amount;
        }

        public Envelope(string Id, string Name, double Goal, double Person1Amount, double Person2Amount)
        {
            this.Id = Id;
            this.Name = Name;
            this.Goal = Goal;
            this.Person1Amount = Person1Amount;
            this.Person2Amount = Person2Amount;
        }

        public Envelope(string Id, string Name, double Goal, double Person1Amount, double Person2Amount, string Person1Account)
        {
            this.Id = Id;
            this.Name = Name;
            this.Goal = Goal;
            this.Person1Amount = Person1Amount;
            this.Person2Amount = Person2Amount;
            this.Person1Account = Person1Account;
        }

        public Envelope(string Id, string Name, double Goal, double Person1Amount, double Person2Amount, string Person1Account, string Person2Account)
        {
            this.Id = Id;
            this.Name = Name;
            this.Goal = Goal;
            this.Person1Amount = Person1Amount;
            this.Person2Amount = Person2Amount;
            this.Person1Account = Person1Account;
            this.Person2Account = Person2Account;
        }

        public Envelope(Envelope other)
        {
            Id = other.Id;
            Name = other.Name;
            Goal = other.Goal;
            UpdateGoal = other.UpdateGoal;
            Person1Amount = other.Person1Amount;
            Person2Amount = other.Person2Amount;
            Person1Account = other.Person1Account;
            Person2Account = other.Person2Account;
        }
    }
}
