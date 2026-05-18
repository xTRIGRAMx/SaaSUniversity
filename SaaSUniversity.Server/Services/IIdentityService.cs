using Microsoft.AspNetCore.Http;
using SaaSUniversity.Server.Controllers;
using SaaSUniversity.Shared;
using static SaaSUniversity.Server.Controllers.AuthController;

namespace SaaSUniversity.Server.Services
{
    public interface IIdentityService
    {
        Task<LoginResult?> RegisterAsync(StudentDto dto, HttpContext httpContext);
        Task<LoginResult?> LoginAsync(StudentDto dto, HttpContext httpContext);
        Task<StudentDto?> ValidateSessionAsync(int studentId);
        Task LogoutAsync(HttpContext httpContext);
    }
}
