namespace App_TODO_API.Models;

public class User(string name, string email, string password)
{
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}