using System.Text.RegularExpressions;

namespace Q2.Helpers
{
    public static class ValidationHelpers
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return EmailRegex.IsMatch(email);
        }

        public static bool TryParseDecimal(string text, out decimal value)
        {
            return decimal.TryParse(text, out value);
        }

        public static bool IsRequired(string text)
        {
            return !string.IsNullOrWhiteSpace(text);
        }
    }
}
