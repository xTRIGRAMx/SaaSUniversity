using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SaaSUniversity.Server.Data;
using SaaSUniversity.Server.Models;
using SaaSUniversity.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SaaSUniversity.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AuthController(AppDbContext context) => _context = context;

        [HttpPost("register")]
        public async Task<ActionResult<StudentDto>> Register(StudentDto dto)
        {
            var student = new Student
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return new StudentDto { Id = student.Id, Email = student.Email };
        }

        [HttpPost("login")]
        public IActionResult Login(StudentDto dto)
        {
            var student = _context.Students.FirstOrDefault(s => s.Email == dto.Email);
            if (student == null || !BCrypt.Net.BCrypt.Verify(dto.Password, student.PasswordHash))
                return Unauthorized();

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, student.Email),
        new Claim("StudentId", student.Id.ToString())
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("ThisIsASuperLongSecretKeyForJWT1234567890"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return Ok(new LoginResult
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                StudentId = student.Id
            });
        }

        public class LoginResult
        {
            public string Token { get; set; } = string.Empty;
            public int StudentId { get; set; }
        }
    }
}
