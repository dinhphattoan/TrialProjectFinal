using ReactApp1.Server.Enitity.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ReactApp1.Server.DTO
{
    public class IdentityUserRegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [PasswordComplexity]
        public string Password { get; set; }

        public IdentityUserRegisterDTO(string email, string username, string password)
        {
            Email = email;
            Username = username;
            Password = password;
        }
    }
}
