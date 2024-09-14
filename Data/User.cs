namespace BetterBudgetWeb.Data
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Person2 { get; set; }
        public string Passkey { get; set; }
        public string Passkey2 { get; set; }
        public string Token { get; set; }
        public string Expiration { get; set; }
        public User() { }
        public User(string id, string name, string person2, string passkey)
        {
            Id = id;
            Name = name;
            Person2 = person2;
            Passkey = passkey;
        }
        public User(User other)
        {
            Id = other.Id;
            Name = other.Name;
            Person2 = other.Person2;
            Passkey = other.Passkey;
            Token = other.Token;
            Expiration = other.Expiration;
        }
    }
}
