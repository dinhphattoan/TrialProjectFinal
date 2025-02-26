using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ReactApp.Server.Entity
{
    public class Glossary
    {
        [Key]
        public Guid Guid { get; set; }
        [Required]
        [MaxLength(50)]
        public string TermOfPhrase { get; set; }
        [Required]
        [MaxLength(500)]
        public string GlossaryExplaination { get; set; }
        public DateTime DateAdded { get; set; }

        //Nav
        public IdentityUser UserCreatedBy { get; set; }

        public Glossary(Guid guid, string termOfPhrase, string glossaryExplaination, DateTime dateAdded)
        {
            Guid = guid;
            TermOfPhrase = termOfPhrase;
            GlossaryExplaination = glossaryExplaination;
            DateAdded = dateAdded;
        }
        public Glossary(string termOfPhrase, string glossaryExplaination)
        {
            Guid = Guid.NewGuid(); 
            TermOfPhrase = termOfPhrase;
            GlossaryExplaination = glossaryExplaination;
            DateAdded = DateTime.UtcNow; 
        }
    }
}
