namespace App_TODO_API.Utils;

public static class PasswordHelper
{
    public static async Task <string> HashPassword(string password)
    {
        return await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password));
    }

    public static async Task<bool> VerifyPassword(string password, string hashedPassword)
    {
        return await Task.Run(() => BCrypt.Net.BCrypt.Verify(password, hashedPassword));
    }
}