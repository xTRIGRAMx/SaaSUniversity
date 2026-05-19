using SaaSUniversity.Shared;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SaaSUniversity.Client.Service
{
    public class AuthService : HttpServiceBase, IAuthService
    {
        public AuthService(HttpClient http) : base(http) { }

        public async Task<StudentDto?> ValidateSessionAsync()
        {
            try
            {
                var request = CreateAuthenticatedRequest(HttpMethod.Get, "api/auth/validate");
                var response = await Http.SendAsync(request);
                return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<StudentDto>() : null;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Network error during session validation: {ex.Message}");
                return null; 
            }
        }

        public async Task<LoginResult?> LoginAsync(StudentDto dto)
        {
            try
            {
                var request = CreateAuthenticatedRequest(HttpMethod.Post, "api/auth/login", dto);
                var response = await Http.SendAsync(request);
                return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<LoginResult>() : null;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Network error during student authentication: {ex.Message}");
                return null; 
            }
        }

        public async Task<LoginResult?> RegisterAsync(StudentDto dto)
        {
            try
            {
                var request = CreateAuthenticatedRequest(HttpMethod.Post, "api/auth/register", dto);
                var response = await Http.SendAsync(request);
                return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<LoginResult>() : null;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Network error during student registration: {ex.Message}");
                return null; 
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                var request = CreateAuthenticatedRequest(HttpMethod.Post, "api/auth/logout");
                var response = await Http.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Network error during session termination: {ex.Message}");
                return false; 
            }
        }
    }
}