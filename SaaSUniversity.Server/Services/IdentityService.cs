using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SaaSUniversity.Server.Data;
using SaaSUniversity.Server.Models;
using SaaSUniversity.Shared;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SaaSUniversity.Server.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly AppDbContext _context;

        public IdentityService(AppDbContext context) => _context = context;

        public async Task<LoginResult?> RegisterAsync(StudentDto dto, HttpContext httpContext)
        {
            try
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
                await _context.SaveChangesAsync(); 

                await IssueAuthenticationCookieAsync(student.Id, student.Email, httpContext);

                return new LoginResult { StudentId = student.Id };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical Exception in IdentityService.RegisterAsync: {ex.Message}");
                return null; 
            }
        }

        public async Task<LoginResult?> LoginAsync(StudentDto dto, HttpContext httpContext)
        {
            try
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == dto.Email);
                if (student == null || !BCrypt.Net.BCrypt.Verify(dto.Password, student.PasswordHash))
                    return null;

                await IssueAuthenticationCookieAsync(student.Id, student.Email, httpContext);

                return new LoginResult { StudentId = student.Id };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical Exception in IdentityService.LoginAsync: {ex.Message}");
                return null; 
            }
        }

        public async Task<StudentDto?> ValidateSessionAsync(int studentId)
        {
            try
            {
                var student = await _context.Students.FindAsync(studentId);
                if (student == null) return null;

                return new StudentDto { Id = student.Id, Email = student.Email };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical Exception in IdentityService.ValidateSessionAsync: {ex.Message}");
                return null; 
            }
        }

        public async Task LogoutAsync(HttpContext httpContext)
        {
            try
            {
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in IdentityService.LogoutAsync: {ex.Message}");
            }
        }

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