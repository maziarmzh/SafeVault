public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }

    // Add roles property
    public List<string> Roles { get; set; } = new List<string>();
}
