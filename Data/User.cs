namespace BetterBudgetWeb.Data
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Passkey { get; set; }
        public string Passkey2 { get; set; }
        public string Token { get; set; }
        public string Expiration { get; set; }
        public User() { }
    }
}
