using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
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

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT password_hash FROM users WHERE username = @u", conn);
            cmd.Parameters.AddWithValue("u", request.Username);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return Unauthorized(new { message = "Invalid Credentials"});

            var storedHash = reader.GetString(reader.GetOrdinal("password_hash"));
            var result = _passwordHasher.VerifyHashedPassword(request.Username, storedHash, request.Password);

            if (result == PasswordVerificationResult.Success)
            {
                var id = reader.GetInt32(reader.GetOrdinal("id"));
                var username = reader.GetString(reader.GetOrdinal("username"));
                return Ok(new {id, username});
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }
    }

}
