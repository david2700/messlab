using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Npgsql;
using MessLabBackend.Models;

namespace MessLabBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly string _connectionString = "Host=localhost;Username=mac;Password=;Database=messlab";
        private readonly PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();

        private readonly string _jwtKey = "THIS_IS_A_32+_CHARACTER_LONG_SECRET_KEY_123456";

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT id, username, password_hash FROM users WHERE username = @u", conn);
            cmd.Parameters.AddWithValue("u", request.Username);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return Unauthorized(new { message = "Invalid Credentials"});

            var storedHash = reader.GetString(reader.GetOrdinal("password_hash"));
            var result = _passwordHasher.VerifyHashedPassword(
                request.Username,
                storedHash,
                request.Password
            );

            if (result != PasswordVerificationResult.Success)
                 return Unauthorized(new { message = "Invalid credentials" });

            var userId = reader.GetInt32(reader.GetOrdinal("id"));
            var username = reader.GetString(reader.GetOrdinal("username"));

            var token = GenerateJwt(userId, username);
            return Ok(new 
            {
                token,
                user = new {id = userId, username}
            });
        }

        private string GenerateJwt(int userId, string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtKey)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
