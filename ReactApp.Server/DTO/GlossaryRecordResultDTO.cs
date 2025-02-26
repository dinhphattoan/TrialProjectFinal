using ReactApp.Server.Entity;

namespace ReactApp.Server.DTO
{
    public class GlossaryRecordResultDTO
    {
        public int total { get; set; } = 0;
        public IEnumerable<Glossary> data { get; set; } = Enumerable.Empty<Glossary>();
        public GlossaryRecordResultDTO() { }
    }
}
