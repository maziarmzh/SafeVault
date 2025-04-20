using System.Text.RegularExpressions;

namespace SafeVault.Services
{
    public class SanitizationService
    {
        public string SanitizeInput(string input)
        {
            // Remove potentially harmful characters
            var sanitized = Regex.Replace(input, @"[<>""';]", string.Empty);
            // Additional security measures can be added here
            return sanitized.Trim();
        }

        public bool IsValidEmail(string email)
        {
            // Simple regex for validating email format
            var emailRegex = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
            return Regex.IsMatch(email, emailRegex);
        }

        public bool IsValidUsername(string username)
        {
            // Ensure username contains only alphanumeric characters
            var usernameRegex = @"^[a-zA-Z0-9]+$";
            return Regex.IsMatch(username, usernameRegex);
        }
    }
}

