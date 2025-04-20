using BCrypt.Net;
using Dapper;
using Microsoft.Data.SqlClient;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        // Example query to fetch user by username
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(); // Ensure the connection is opened
        var query = "SELECT Id, Username, PasswordHash FROM Users WHERE Username = @Username";
        return await connection.QueryFirstOrDefaultAsync<User>(
            query, new { Username = username });
    }

    public async Task<bool> AuthenticateUserAsync(string username, string password)
    {
        var user = await GetUserByUsernameAsync(username);
        if (user == null)
        {
            return false; // User not found
        }

        // Compare the provided password with the stored hash
        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    public async Task<bool> RegisterUserAsync(string username, string password)
    {
        // Hash the password before storing it
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(); // Ensure the connection is opened
        var query = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
        var result = await connection.ExecuteAsync(query, new { Username = username, PasswordHash = passwordHash });

        return result > 0; // Return true if the user was successfully registered
    }

    internal async Task<object?> GetUserByUsernameAsync(string maliciousUsername, string maliciousPassword)
    {
        // Hash the provided password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(maliciousPassword);

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(); // Ensure the connection is opened

        // Query to fetch user by username and hashed password
        var query = "SELECT Id, Username, PasswordHash FROM Users WHERE Username = @Username AND PasswordHash = @PasswordHash";
        return await connection.QueryFirstOrDefaultAsync<User>(
            query, new { Username = maliciousUsername, PasswordHash = passwordHash });
    }
}
