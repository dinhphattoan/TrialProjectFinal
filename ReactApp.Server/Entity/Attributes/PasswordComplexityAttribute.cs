using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ReactApp1.Server.Enitity.Attributes
{
    public class PasswordComplexityAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return false;
            string password = value.ToString() ?? string.Empty;
            string specialCharacterRegex = @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]";
            bool hasSpecialChar = Regex.IsMatch(password, specialCharacterRegex);
            bool hasUpperCaseAtFirstChar = char.IsUpper(password[0]);
            bool hasNumberChar = Regex.IsMatch(password, @"[0-9]");
            return hasSpecialChar && hasUpperCaseAtFirstChar && hasNumberChar;
        }
        public override string FormatErrorMessage(string name)
        {
            return $"Password must contain at least one special character, one uppercase letter, one lowercase letter, and one number.";
        }
    }
}
