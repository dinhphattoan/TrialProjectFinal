namespace ReactApp.Server.Contracts.DTOs.Glossaries
{
    public class GlossaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Definition { get; set; } = string.Empty;
        public DateTime? CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
    }
}
