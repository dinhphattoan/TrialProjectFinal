using Microsoft.AspNetCore.Identity;
using ReactApp.Server.Services.Interface;
using System.Data.Common;

namespace ReactApp.Server.Services
{
    public class IdentityUserService : IIdentityUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<IdentityUserService> _logger;
        public IdentityUserService(UserManager<IdentityUser> userManager, ILogger<IdentityUserService> logger)
        {
            _userManager = userManager;
            _logger = logger;

        }
        public async Task<IdentityResult> AddUserAsync(IdentityUser identityUser, string password, CancellationToken cancellation =default)
        {
            try
            {
                IdentityResult identityResult = await _userManager.CreateAsync(identityUser, password);
                if (!identityResult.Succeeded)
                {
                    _logger.LogError("User creation failed for email={Email}. Errors: {Errors}",
               identityUser.Email, string.Join(", ", identityResult.Errors.Select(e => e.Description)));
                }
                return identityResult;
            }
            catch (DbException e)
            {
                _logger.LogError(e, "Database error occurred while create user with email={Email}", identityUser.Email);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error occured while create user with email={Email}", identityUser.Email);
                throw;
            }
        }

        public Task<IdentityUser?> GetSignedInUserAsync(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser?> GetUserByEmailAsync(string email, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }
    }
}
