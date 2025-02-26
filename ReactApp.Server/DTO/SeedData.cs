using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReactApp.Server.Data;
using ReactApp.Server.Entity;
using System.Security.Claims;
using System.Text;

namespace ReactApp1.Server.Data
{
    public static class SeedData
    {
        public static async Task InitializeUserRole(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                IServiceProvider scopeProvider = scope.ServiceProvider;
                RoleManager<IdentityRole> roleManager = scopeProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roleNames = new[] { "Admin", "User", "Manager" };
                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        var role = new IdentityRole { Name = roleName };
                        await roleManager.CreateAsync(role);
                    }
                }
            }

        }

        static readonly string fileGlossaryRecordPath = "C:\\Users\\toan.dinh\\source\\repos\\ReactApp\\ReactApp.Server\\Data\\GlossaryReport.txt";
        public async static Task InitialGlossaryRecord(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                IServiceProvider provider = scope.ServiceProvider;
                ILogger<ApplicationDbContext> logger = provider.GetRequiredService<ILogger<ApplicationDbContext>>();
                UserManager<IdentityUser> userManager = provider.GetRequiredService<UserManager<IdentityUser>>();
                ApplicationDbContext dbContext = provider.GetRequiredService<ApplicationDbContext>();

                var glossaries = await dbContext.Glossaries.ToListAsync();
                dbContext.Glossaries.RemoveRange(glossaries);

                try
                {
                    using (FileStream fs = new FileStream(fileGlossaryRecordPath, FileMode.Open, FileAccess.Read))
                    {
                        IdentityUser? identityUser = await userManager.Users.FirstOrDefaultAsync();
                        if (identityUser == null)
                        {
                            logger.LogWarning("No user found in the database.");
                            return;
                        }

                        byte[] bytes = new byte[fs.Length];
                        await fs.ReadExactlyAsync(bytes);
                        string readText = Encoding.UTF8.GetString(bytes);
                        string[] stringLines = readText.Split('\n');

                        for (int i = 1; i < stringLines.Length; i++)
                        {
                            string[] stringParts = stringLines[i].Split(':');
                            if (stringParts.Length == 2 && !string.IsNullOrEmpty(stringParts[0]) && !string.IsNullOrEmpty(stringParts[1]))
                            {
                                var newGlossary = new Glossary(Guid.NewGuid(), stringParts[0], stringParts[1].Trim(), DateTime.UtcNow, identityUser)
                                {
                                    UserCreatedBy = identityUser
                                };
                                await dbContext.Glossaries.AddAsync(newGlossary);
                            }
                            else
                            {
                                logger.LogWarning($"Invalid line format: {stringLines[i]}");
                            }
                        }
                    }

                    await dbContext.SaveChangesAsync();
                }
                catch (FileNotFoundException ex)
                {
                    logger.LogError(ex, "File not found.");
                }
                catch (IOException ex)
                {
                    logger.LogError(ex, "Error reading from file.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An unexpected error occurred during glossary initialization.");

                }
            }
        }
    }
}
