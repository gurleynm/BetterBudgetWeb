namespace BetterBudgetWeb.Data
{
    public class Bucket
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double Person1Goal { get; set; }

        public double Person2Goal { get; set; }

        public double TotalGoal => Person1Goal + Person2Goal;

        public double Person1Amount { get; set; }

        public double Person2Amount { get; set; }

        public double TotalAmount => Person1Amount + Person2Amount;

        public Bucket() { }

        public Bucket(string Id)
        {
            this.Id = Id;
        }

        public Bucket(string Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }

        public Bucket(string Id, string Name, double Person1Goal)
        {
            this.Id = Id;
            this.Name = Name;
            this.Person1Goal = Person1Goal;
        }

        public Bucket(string Id, string Name, double Person1Goal, double Person2Goal)
        {
            this.Id = Id;
            this.Name = Name;
            this.Person1Goal = Person1Goal;
            this.Person2Goal = Person2Goal;
        }

        public Bucket(string Id, string Name, double Person1Goal, double Person2Goal, double Person1Amount)
        {
            this.Id = Id;
            this.Name = Name;
            this.Person1Goal = Person1Goal;
            this.Person2Goal = Person2Goal;
            this.Person1Amount = Person1Amount;
        }

        public Bucket(string Id, string Name, double Person1Goal, double Person2Goal, double Person1Amount, double Person2Amount)
        {
            this.Id = Id;
            this.Name = Name;
            this.Person1Goal = Person1Goal;
            this.Person2Goal = Person2Goal;
            this.Person1Amount = Person1Amount;
            this.Person2Amount = Person2Amount;
        }

        public Bucket(Bucket other)
        {
            this.Id = other.Id;
            this.Name = other.Name;
            this.Person1Goal = other.Person1Goal;
            this.Person2Goal = other.Person2Goal;
            this.Person1Amount = other.Person1Amount;
            this.Person2Amount = other.Person2Amount;
        }
    }
}
