using App_TODO_API.Database;
using App_TODO_API.Models;
using App_TODO_API.Utils;

namespace App_TODO_API.Services;

public class RegisterService
{
    public async Task<bool> Register(string name, string email, string password)
    {
        // Check if user already exists
        if (await DbManager.UserExists(email))
        {
            return false;
        }

        // Hash the password
        await PasswordHelper.HashPassword(password);

        // Create a new user
        var user = new User(name, email, password);

        // Save the user to the database
        await DbManager.AddUser(user);

        return true;
    }
}