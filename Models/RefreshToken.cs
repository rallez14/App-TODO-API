namespace App_TODO_API.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public int? UserId { get; set; }
    public DateTime Expires { get; set; }
    public bool Revoked { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
