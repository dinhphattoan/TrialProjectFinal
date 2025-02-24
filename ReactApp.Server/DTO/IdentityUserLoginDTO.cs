using System.ComponentModel.DataAnnotations;
namespace ReactApp1.Server.DTO
{
    public class IdentityUserLoginDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public IdentityUserLoginDTO(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
