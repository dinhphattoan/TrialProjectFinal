using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ReactApp.Server.Entity
{
    public class Glossary : BaseEntity<Guid>
    {
        [Required]
        [MaxLength(50)]
        public string TermOfPhrase { get; set; }
        [Required]
        [MaxLength(500)]
        public string GlossaryExplaination { get; set; }
        public DateTime DateAdded { get; set; }
        
        public string CreateById { get; set; }
        public IdentityUser UserCreatedBy { get; set; }
    }
}
