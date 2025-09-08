using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using Services;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IUserRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest req)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Email and password are required.");


            var existing = await _repo.GetByEmailAsync(req.Email);
            if (existing != null) return Conflict("Email already registered.");


            PasswordHasher.CreatePasswordHash(req.Password, out var hash, out var salt);


            var user = new User
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            var userId = await _repo.CreateAsync(user);
            return CreatedAtAction(nameof(SignUp), new { id = userId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            if (req.Email != null)
            {
                var user = await _repo.GetByEmailAsync(req.Email);
                if (user == null) return Unauthorized("Invalid credentials.");

                if (req.Password != null && user.PasswordHash != null && user.PasswordSalt != null && !PasswordHasher.VerifyPassword(req.Password, user.PasswordHash, user.PasswordSalt))
                    return Unauthorized("Invalid credentials.");

                // create token
                var jwt = _config.GetSection("Jwt");
                var key = Encoding.UTF8.GetBytes(jwt.GetValue<string>("Key") ?? string.Empty);
                var tokenHandler = new JwtSecurityTokenHandler();

                var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email!),
                    new Claim("firstName", user.FirstName!),
                    new Claim("lastName", user.LastName!)
                };

                var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
                var expires = DateTime.UtcNow.AddMinutes(jwt.GetValue<int>("ExpireMinutes"));

                var token = new JwtSecurityToken(
                    issuer: jwt.GetValue<string>("Issuer"),
                    audience: jwt.GetValue<string>("Audience"),
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds
                );

                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new AuthResponse { Token = tokenString, ExpiresAt = expires });
            }

            return BadRequest("Email is required.");
        }
    }
}
