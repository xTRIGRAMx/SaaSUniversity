using SaaSUniversity.Shared;
using System.Net.Http.Json;

namespace SaaSUniversity.Client.Service
{
    public class AuthService : HttpServiceBase, IAuthService
    {
        public AuthService(HttpClient http) : base(http) { }

        public async Task<StudentDto?> ValidateSessionAsync()
        {
            var request = CreateAuthenticatedRequest(HttpMethod.Get, "api/auth/validate");
            var response = await Http.SendAsync(request);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<StudentDto>() : null;
        }

        public async Task<LoginResult?> LoginAsync(StudentDto dto)
        {
            var request = CreateAuthenticatedRequest(HttpMethod.Post, "api/auth/login", dto);
            var response = await Http.SendAsync(request);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<LoginResult>() : null;
        }

        public async Task<LoginResult?> RegisterAsync(StudentDto dto)
        {
            var request = CreateAuthenticatedRequest(HttpMethod.Post, "api/auth/register", dto);
            var response = await Http.SendAsync(request);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<LoginResult>() : null;
        }

        public async Task<bool> LogoutAsync()
        {
            var request = CreateAuthenticatedRequest(HttpMethod.Post, "api/auth/logout");
            var response = await Http.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
