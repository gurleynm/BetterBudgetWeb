namespace BetterBudgetWeb.Data
{
    public class TokenHolder
    {
        public string Name { get; set; }
        public string Person2 { get; set; }
        public string Token { get; set; }
        public TokenHolder() { }
        public TokenHolder(User user)
        {
            Name = user.Name;
            Person2 = user.Person2;
        }
        public TokenHolder(User user, string token)
        {
            Name = user.Name;
            Person2 = user.Person2;
            Token = token;
        }
        public TokenHolder(TokenHolder other)
        {
            Name = other.Name;
            Person2 = other.Person2;
            Token = other.Token;
        }
    }
}
