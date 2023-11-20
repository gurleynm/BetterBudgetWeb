namespace BetterBudgetWeb.Data
{
    public class TokenHolder
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Person2 { get; set; }
        public string Passkey { get; set; }
        public string Token { get; set; }
        public string GenerateTime { get; set; }
        public TokenHolder() { }
        public TokenHolder(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Person2 = user.Person2;
            Passkey = user.Passkey;
        }
        public TokenHolder(User user, string token)
        {
            Id = user.Id;
            Name = user.Name;
            Person2 = user.Person2;
            Passkey = user.Passkey;
            Token = token;
            GenerateTime = DateTime.Now.ToString();
        }
        public TokenHolder(TokenHolder other)
        {
            Id = other.Id;
            Name = other.Name;
            Person2 = other.Person2;
            Passkey = other.Passkey;
            Token = other.Token;
            GenerateTime = other.GenerateTime;
        }
    }
}
