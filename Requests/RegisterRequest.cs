namespace App_TODO_API.Requests;

public class RegisterRequest(string name, string email, string password)
{
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}