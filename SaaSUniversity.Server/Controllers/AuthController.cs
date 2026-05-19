using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SaaSUniversity.Server.Data;
using SaaSUniversity.Server.Extensions;
using SaaSUniversity.Server.Models;
using SaaSUniversity.Server.Services;
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
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService) => _identityService = identityService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(StudentDto dto)
        {
            var result = await _identityService.RegisterAsync(dto, HttpContext);
            if (result == null)
            {
                return BadRequest("A student with this email already exists. Please log in instead.");
            }
            return Ok(result); // Returns LoginResult with StudentId + auto-login cookie attached
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(StudentDto dto)
        {
            var result = await _identityService.LoginAsync(dto, HttpContext);
            return result == null ? Unauthorized() : Ok(result);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("validate")]
        public async Task<ActionResult<StudentDto>> Validate()
        {
            var studentId = User.GetStudentId();
            if (studentId == null)
                return Unauthorized("Student ID not found in cookie.");

            var student = await _identityService.ValidateSessionAsync(studentId.Value);
            if (student == null)
                return Unauthorized("Student records missing (Database may have restarted).");

            return Ok(student);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _identityService.LogoutAsync(HttpContext);
            return Ok();
        }
    }
}
