using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaaSUniversity.Server.Extensions;
using SaaSUniversity.Server.Services;
using SaaSUniversity.Shared;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

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
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var result = await _identityService.RegisterAsync(dto, HttpContext);
                if (result == null)
                {
                    return BadRequest("A student with this email already exists or the registration request was rejected.");
                }
                return Ok(result); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Controller Exception in Auth.Register: {ex.Message}");
                return StatusCode(500, "An internal error occurred during student profile generation.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(StudentDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var result = await _identityService.LoginAsync(dto, HttpContext);
                return result == null
                    ? Unauthorized("Invalid email address or password configuration.")
                    : Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Controller Exception in Auth.Login: {ex.Message}");
                return StatusCode(500, "An internal processing error occurred during authentication routing.");
            }
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("validate")]
        public async Task<ActionResult<StudentDto>> Validate()
        {
            try
            {
                var studentId = User.GetStudentId();
                if (studentId == null)
                    return Unauthorized("Student ID not found in cookie validation token.");

                var student = await _identityService.ValidateSessionAsync(studentId.Value);
                if (student == null)
                    return Unauthorized("Student records missing or database sync lost.");

                return Ok(student);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Controller Exception in Auth.Validate: {ex.Message}");
                return StatusCode(500, "An internal error occurred while validating user security parameters.");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _identityService.LogoutAsync(HttpContext);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Controller Exception in Auth.Logout: {ex.Message}");
                return StatusCode(500, "An internal error occurred during sign out processing.");
            }
        }
    }
}