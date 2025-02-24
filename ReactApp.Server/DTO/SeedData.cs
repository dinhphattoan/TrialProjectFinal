using Microsoft.AspNetCore.Identity;

namespace ReactApp1.Server.Data
{
    public static class SeedData
    {
        public static async Task InitializeUserRole(IServiceProvider serviceProvider)
        {
            using(var scope = serviceProvider.CreateScope())
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
        
    }
}
