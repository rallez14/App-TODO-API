namespace App_TODO_API.Models;

public class User(int? Id, string name, string email, string password)
{
    public int? Id { get; set; } = Id;
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}