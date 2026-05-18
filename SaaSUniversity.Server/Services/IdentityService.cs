using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SaaSUniversity.Server.Controllers;
using SaaSUniversity.Server.Data;
using SaaSUniversity.Server.Models;
using SaaSUniversity.Shared;
using System.Security.Claims;

namespace SaaSUniversity.Server.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly AppDbContext _context;

        public IdentityService(AppDbContext context) => _context = context;

        public async Task<LoginResult?> RegisterAsync(StudentDto dto, HttpContext httpContext)
        {
            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.Email == dto.Email);

            if (existingStudent != null) return null;

            var student = new Student
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync(); // 🚨 Persist immediately to generate the student.Id

            // 🔑 Trigger automatic sign-in workflow instantly post-registration
            await IssueAuthenticationCookieAsync(student.Id, student.Email, httpContext);

            return new LoginResult { StudentId = student.Id };
        }

        public async Task<LoginResult?> LoginAsync(StudentDto dto, HttpContext httpContext)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == dto.Email);
            if (student == null || !BCrypt.Net.BCrypt.Verify(dto.Password, student.PasswordHash))
                return null;

            await IssueAuthenticationCookieAsync(student.Id, student.Email, httpContext);

            return new LoginResult { StudentId = student.Id };
        }

        public async Task<StudentDto?> ValidateSessionAsync(int studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null) return null;

            return new StudentDto { Id = student.Id, Email = student.Email };
        }

        public async Task LogoutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        // Shared internal method to completely eliminate copy-pasted cookie generation logic
        private static async Task IssueAuthenticationCookieAsync(int studentId, string email, HttpContext httpContext)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim("StudentId", studentId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}
