using System.ComponentModel.DataAnnotations;

namespace ReactApp.Server.DTO.Glossary
{
    public class AddGlossaryDto
    {
        [MaxLength(50, ErrorMessage = "50 characters length exceeded!")]
        public required string TermOfPhrase { get; set; }
        [MaxLength(500, ErrorMessage = "500 characters length exceeded!")]
        public required string Explaination { get; set; }
        [Required]
        public required string CreatedBy { get; set; }

    }
}
