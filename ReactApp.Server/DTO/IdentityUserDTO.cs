namespace ReactApp1.Server.DTO
{
    public class IdentityUserDTO
    {
        public string? Email { get; set; }
        public string? Username { get; set; }
        public DateTime? DateCreated { get; set; }
        public IdentityUserDTO() { }
        public IdentityUserDTO(string email, string username, DateTime dateCreated)
        {
            Email = email;
            Username = username;
            DateCreated = dateCreated;
        }
    }
}
