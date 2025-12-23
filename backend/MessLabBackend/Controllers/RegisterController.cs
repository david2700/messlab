using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Npgsql;
using MessLabBackend.Models;

namespace MessLabBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly string _connectionString = "Host=localhost;Username=mac;Password=;Database=messlab";
        private readonly PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();

        [HttpPost]
        public IActionResult Register([FromBody] LoginRequest request)
        {
            var hashedPassword = _passwordHasher.HashPassword(request.Username, request.Password);

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand("INSERT INTO users (username, password_hash) VALUES (@u, @p)", conn);
            cmd.Parameters.AddWithValue("u", request.Username);
            cmd.Parameters.AddWithValue("p", hashedPassword);
            cmd.ExecuteNonQuery();

            return Ok(new { message = "User registered successfully" });
        }
    }
}


