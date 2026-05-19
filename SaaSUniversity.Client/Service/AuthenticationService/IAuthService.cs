using SaaSUniversity.Shared;

namespace SaaSUniversity.Client.Service
{
    public interface IAuthService
    {
        Task<StudentDto?> ValidateSessionAsync();
        Task<LoginResult?> LoginAsync(StudentDto dto);
        Task<LoginResult?> RegisterAsync(StudentDto dto);
        Task<bool> LogoutAsync();
    }
}
