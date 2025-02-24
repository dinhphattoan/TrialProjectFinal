using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ReactApp.Server.Data;
using ReactApp1.Server.DTO;
namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountsController(ApplicationDbContext clientOrderDbContext,
        SignInManager<IdentityUser> signInManager,
        ILogger<AccountsController> logger,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration) : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;
        private readonly ApplicationDbContext _clientOrderDbContext = clientOrderDbContext;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly ILogger<AccountsController> _logger = logger;
        private readonly IConfiguration _configuration = configuration;
        private readonly bool IS_PASSWORD_PERSISTENT = true;

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAccountAsync([FromBody] IdentityUserLoginDTO identityUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid request data." });
            }

            var identityUserFind = await _userManager.Users
                .FirstOrDefaultAsync(p => p.UserName == identityUserDTO.Username);

            if (identityUserFind == null)
            {
                return Unauthorized(new { Message = "Invalid username or password." });
            }

            var signInResult = await _signInManager.PasswordSignInAsync(identityUserFind, identityUserDTO.Password, IS_PASSWORD_PERSISTENT, lockoutOnFailure: true);

            if (signInResult.IsLockedOut)
            {
                return Unauthorized(new { Message = "Your account is locked. Please try again later." });
            }
            if (signInResult.RequiresTwoFactor)
            {
                return Unauthorized(new { Message = "Two-factor authentication is required." });
            }
            if(!signInResult.Succeeded)
            {
                return Unauthorized(new { Message = "Invalid username or password." });
            }
            string token = GenerateToken(identityUserFind);
            return Ok(new { Message = "Signed in successfully.", token = token });
        }


        [HttpGet("authenticated")]
        [AllowAnonymous]
        public IActionResult IsLoggedIn()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                string username = User.FindFirstValue(ClaimTypes.Name) ?? "Unknown";
                return Ok(new { IsLoggedIn = true, Username = username });
            }
            return BadRequest(new { IsLoggedIn = false });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAccountAsync()
        {
            if (User.Identities == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { Message = "No user is currently logged in." });
            }

            string loggedInUsername = User.Identity?.Name ?? "Unknown User";

            try
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation($"User {loggedInUsername} logged out successfully.");
                return Ok(new { Message = "Logged out successfully!" });
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError($"Logout failed for user {loggedInUsername}: {e.Message}");
                return StatusCode(500, new { Message = "An error occurred while logging out." });
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAccountAsync([FromBody] IdentityUserRegisterDTO identityUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid request data." });
            }
            IdentityUser? existedIdentityUser = await _userManager.Users.FirstOrDefaultAsync(p => p.UserName == identityUserDTO.Username);
            if (existedIdentityUser is not null)
            {
                return BadRequest(new { Message = "Username already exists." });
            }

            IdentityUser registerIdentityUser = new() { Email = identityUserDTO.Email, UserName = identityUserDTO.Username, };
            IdentityResult? registerResult = await _signInManager.UserManager.CreateAsync(registerIdentityUser, identityUserDTO.Password);

            if (registerResult.Errors.Any())
            {
                List<string> errors = new List<string>();
                foreach (var error in registerResult.Errors)
                {
                    errors.Add(error.Description);
                }
                _logger.LogError($"Failed to sign up with Username {registerIdentityUser.UserName}. Error:{string.Join(",", errors)}");
                return BadRequest(new { Message = "User registration failed", Errors = errors });
            }

            _logger.LogInformation($"New user {registerIdentityUser.UserName} registered with Email: {registerIdentityUser.Email}");

            IdentityResult newUserRoleResult = await _userManager.AddToRoleAsync(registerIdentityUser, "User");
            if (newUserRoleResult.Errors.Any())
            {
                List<string> errors = new List<string>();
                foreach (var error in registerResult.Errors)
                {
                    errors.Add(error.Description);
                }
                _logger.LogError($"Failed to assign role to User with {registerIdentityUser.UserName}. Error:{string.Join(",", errors)}");
                return BadRequest(new { Message = "User role assign failed", Errors = errors });
            }
            return CreatedAtAction(nameof(RegisterAccountAsync), new { id = registerIdentityUser.Id }
            , new { Message = $"User register successfully with Role 'User'." });


        }


        [HttpGet("GetAllAccounts")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAccounts()
        {
            try
            {
                List<IdentityUser> identityUsers = await _clientOrderDbContext.Users.ToListAsync();
                List<IdentityUserDTO> identityUserDTOs = identityUsers.Select(account =>
                new IdentityUserDTO()
                {
                    Username = account.UserName,
                    Email = account.Email
                }).ToList();
                return Ok(identityUserDTOs);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting all account in account controller GetAllAccount");
                return StatusCode(500, "An error occurred while retrieving account.");
            }
        }

        [HttpGet("Account/{username}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetEmailFromUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest(new { Message = "Username cannot be empty" });
            }

            IdentityUser? identityUserFind = await _userManager.Users.FirstOrDefaultAsync(p => p.UserName == username);
            if (identityUserFind == null)
            {
                return BadRequest(new { Message = "Account doesn't exist." });
            }
            if (!identityUserFind.EmailConfirmed)
            {
                return Unauthorized(new { Message = "Account Email isn't authorized" });
            }
            return Ok(new { Username = username, Email = identityUserFind.Email });
        }

        private string GenerateToken(IdentityUser identityUser, IEnumerable<Claim>? additionalClaim = null)
        {
            if (identityUser == null)
            {
                throw new ArgumentException(nameof(identityUser), "User cannot be null");
            }
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, identityUser.UserName)
            };
            if (additionalClaim != null)
            {
                claims.AddRange(additionalClaim);
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
