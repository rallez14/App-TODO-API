using App_TODO_API.Models;
using App_TODO_API.Utils;
using System.Data.SQLite;

namespace App_TODO_API.Database;

public class DbManager
{
    public static SQLiteConnection GetConnection()
    {
        const string dbPath = @"C:\Users\rallez\dev\App-TODO-API\Database\Database.db";
        const string connectionString = $"Data Source={dbPath};Version=3;";
        var connection = new SQLiteConnection(connectionString);
        try
        {
            connection.Open();
            return connection;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task AddUser(User user)
    {
        var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Users (Name, Email, Password, CreatedAt) VALUES (@Name, @Email, @Password, @CreatedAt)";
        command.Parameters.AddWithValue("@Name", user.Name);
        command.Parameters.AddWithValue("@Email", user.Email);
        command.Parameters.AddWithValue("@Password", user.Password);
        command.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);
        command.ExecuteNonQuery();
        connection.Close();
    }

    public static async Task<bool> UserExists(string email)
    {
        var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
        command.Parameters.AddWithValue("@Email", email);
        var result = (long)command.ExecuteScalar();
        connection.Close();
        return result > 0;
    }
}