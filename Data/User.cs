namespace BetterBudgetWeb.Data
{
    public class User
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Passkey { get; set; } = "";
        public string Token { get; set; } = "";
        public User() { }
        public User(string name, string email, string pass)
        {
            Name = name;
            Email = email;
            Passkey = pass;
            Token = "";
        }
        public User(User other)
        {
            Id = other.Id;
            Name = other.Name;
            Email = other.Email;
            Passkey = other.Passkey;
            Token = other.Token;
        }
    }
}
