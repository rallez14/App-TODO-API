using App_TODO_API.Models;
using App_TODO_API.Utils;
using System.Data.SQLite;

namespace App_TODO_API.Database;

public abstract class DbManager
{
    private static SQLiteConnection GetConnection()
    {
        const string dbPath = @"C:\Users\rallez\dev\App-TODO-API\Database\Database.db";
        const string connectionString = $"Data Source={dbPath};Version=3;BusyTimeout=5000;";
        var connection = new SQLiteConnection(connectionString);
        connection.Open();

        using var pragma = connection.CreateCommand();
        pragma.CommandText = "PRAGMA journal_mode=WAL;";
        pragma.ExecuteNonQuery();

        return connection;
    }


    public static async Task AddUser(User user)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Users (Name, Email, Password, CreatedAt) VALUES (@Name, @Email, @Password, @CreatedAt)";
        command.Parameters.AddWithValue("@Name", user.Name);
        command.Parameters.AddWithValue("@Email", user.Email);
        command.Parameters.AddWithValue("@Password", user.Password);
        command.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);
        command.ExecuteNonQuery();
    }

    public static async Task<bool> UserExists(string email)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
        command.Parameters.AddWithValue("@Email", email);
        var result = (long)command.ExecuteScalar();
        return result > 0;
    }

    public static async Task<User?> GetUserByEmail(string email)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Users WHERE Email = @Email";
        command.Parameters.AddWithValue("@Email", email);
        var reader = command.ExecuteReader();
        if (reader.Read())
        {
            var user = new User(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3)
            );
            return user;
        }
        return null;
    }

    public static async Task StoreRefreshToken(RefreshToken token)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = @"
        INSERT INTO RefreshTokens (Token, UserId, Expires, CreatedAt, Revoked)
        VALUES (@Token, @UserId, @Expires, @CreatedAt, @Revoked)";

        command.Parameters.AddWithValue("@Token", token.Token);
        command.Parameters.AddWithValue("@UserId", token.UserId);
        command.Parameters.AddWithValue("@Expires", token.Expires.ToString("o"));
        command.Parameters.AddWithValue("@CreatedAt", token.CreatedAt.ToString("o"));
        command.Parameters.AddWithValue("@Revoked", token.Revoked ? 1 : 0);

        await command.ExecuteNonQueryAsync();
    }

}