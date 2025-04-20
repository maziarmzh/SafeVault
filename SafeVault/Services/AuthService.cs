using System.Security.Claims;  
using Microsoft.AspNetCore.Components.Authorization;
using BCrypt.Net;

namespace SafeVault.Services
{ 
    public class AuthService
    {
        private readonly List<User> _users = new(); // Simulated user store

        public AuthService()
        {
            // Example: Add a test user with a hashed password
            _users.Add(new User
            {
                Username = "testuser",
                PasswordHash = HashPassword("password123")
            });
        }

        // Hash a password securely
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Verify a user's credentials
        public bool Authenticate(string username, string password)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            if (user == null) return false;

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }
    }
}  
