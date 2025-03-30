using App_TODO_API.Database;
using App_TODO_API.Models;
using App_TODO_API.Utils;

namespace App_TODO_API.Services;

public class LoginService(IConfiguration config)
{
    public async Task<Token?> Login(string email, string password)
    {
        // Check if user exists
        var user = await DbManager.GetUserByEmail(email);
        if (user == null)
        {
            return null;
        }

        // Verify password
        if (!await PasswordHelper.VerifyPassword(password, user.Password))
        {
            return null;
        }

        // Generate token
        var accessToken = TokenHelper.GenerateAccessToken(user, config);
        var refreshToken = TokenHelper.GenerateRefreshToken(user, config);
        var expiration = DateTime.UtcNow.AddHours(1);

        // Store refresh token in the database
        await DbManager.StoreRefreshToken(refreshToken);

        // Return token
        return new Token(accessToken, refreshToken.Token, expiration);
    }
}