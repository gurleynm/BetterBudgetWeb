namespace BetterBudgetWeb.Data
{
    public class TokenHolder
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Person2 { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Status { get; set; }
        public string Tier { get; set; }
        public TokenHolder() { }
        public TokenHolder(User user)
        {
            Name = user.Name;
        }
        public TokenHolder(User user, string token)
        {
            Name = user.Name;
            Token = token;
        }
        public TokenHolder(TokenHolder other)
        {
            Id = other.Id;
            Name = other.Name;
            Person2 = other.Person2;
            Token = other.Token;
            Status = other.Status;
            Tier = other.Tier;
        }
    }
}
