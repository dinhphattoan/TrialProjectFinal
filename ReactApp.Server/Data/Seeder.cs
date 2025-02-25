using Microsoft.EntityFrameworkCore;
using ReactApp.Server.Entity;
using System.Text;

namespace ReactApp.Server.Data
{
    public static class Seeder
    {
        static readonly string fileGlossaryRecordPath = "C:\\Users\\toan.dinh\\source\\repos\\ReactApp\\ReactApp.Server\\Data\\GlossaryReport.txt";
        public async static void InitialGlossaryRecord(ApplicationDbContext dbContext)
        {
            if (!dbContext.Glossaries.Any())
            {
                IEnumerable<Glossary> glossaries = await dbContext.Glossaries.ToListAsync();
                dbContext.Glossaries.RemoveRange(glossaries);
                try
                {
                    using (FileStream fs = new FileStream(fileGlossaryRecordPath, FileMode.Open, FileAccess.Read)) 
                    {
                        byte[] bytes = new byte[fs.Length]; 
                        fs.ReadExactly(bytes); 
                        string readText = Encoding.UTF8.GetString(bytes);
                        Console.WriteLine($"Data read from file: {readText}");
                        string[] stringLines = readText.Split('\n');
                        List<Glossary> newGlossaries = new();
                        foreach(string line in stringLines)
                        {
                            string[] stringParts = line.Split(':');
                            await dbContext.Glossaries.AddAsync(new Glossary(Guid.NewGuid(), stringParts[0], stringParts[1].Remove(0), DateTime.UtcNow));
                        }
                        
                    }
                    await dbContext.SaveChangesAsync();
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine($"File not found: {ex.Message}");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error reading from file: {ex.Message}");
                }
            }
        }
    }
}
