using System.Text.RegularExpressions;

namespace Assignment.Shared
{
    public static class ValidationHelper
    {
        private static readonly Regex PhoneRegex = new Regex(@"^\d{9,12}$", RegexOptions.Compiled);
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            return PhoneRegex.IsMatch(phone.Trim());
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return EmailRegex.IsMatch(email.Trim());
        }
    }
}
