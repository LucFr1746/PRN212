using System.Text.RegularExpressions;

namespace Q2.Helpers
{
    /// <summary>
    /// Utility class containing quick static validations for WPF forms.
    /// </summary>
    public static class ValidationHelpers
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Validates if a string is a correctly formatted email address.
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return EmailRegex.IsMatch(email);
        }

        /// <summary>
        /// Parses a string to decimal. Returns false if parsing fails.
        /// </summary>
        public static bool TryParseDecimal(string text, out decimal value)
        {
            return decimal.TryParse(text, out value);
        }

        /// <summary>
        /// Parses a string to int. Returns false if parsing fails.
        /// </summary>
        public static bool TryParseInt(string text, out int value)
        {
            return int.TryParse(text, out value);
        }

        /// <summary>
        /// Validates that a string is not null, empty, or whitespace.
        /// </summary>
        public static bool IsRequired(string text)
        {
            return !string.IsNullOrWhiteSpace(text);
        }
    }
}
