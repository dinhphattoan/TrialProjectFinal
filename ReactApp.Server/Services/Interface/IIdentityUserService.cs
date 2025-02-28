using Microsoft.AspNetCore.Identity;

namespace ReactApp.Server.Services.Interface
{
    public interface IIdentityUserService 
    {
        public Task<IdentityUser?> GetUserByEmailAsync(string email,CancellationToken cancellation);
        public Task<IdentityUser?> GetSignedInUserAsync(CancellationToken cancellation);
        public Task<IdentityResult> AddUserAsync(IdentityUser identityUser, string password, CancellationToken cancellation=default);
        
    }
}
