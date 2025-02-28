using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ReactApp.Server.Repository.Interface
{
    public interface IIdentityUserRepository
    {
        public Task<IdentityUser?> GetUserByEmailAsync(string email, CancellationToken cancellation);
        public Task<IdentityUser?> GetSignedInUserAsync(ClaimsPrincipal claimsPrincipal,CancellationToken cancellation);

    }
}
