using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using SaaSUniversity.Shared;

namespace SaaSUniversity.Client.Service
{
    public class CookieAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthService _authService;
        private readonly UserState _userState;
        private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CookieAuthenticationStateProvider(IAuthService authService, UserState userState)
        {
            _authService = authService;
            _userState = userState;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // 🔑 Call the service layer instead of raw HttpClient messages
                var student = await _authService.ValidateSessionAsync();

                if (student != null)
                {
                    _userState.SetEmail(student.Email);

                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, student.Email),
                        new Claim("StudentId", student.Id.ToString())
                    };
                    var identity = new ClaimsIdentity(claims, "CookieAuth");
                    return new AuthenticationState(new ClaimsPrincipal(identity));
                }
            }
            catch (Exception)
            {
                // Fall through to anonymous state safely
            }

            _userState.Clear();
            return new AuthenticationState(_anonymous);
        }

        public void NotifyUserAuthentication(string email, int studentId)
        {
            var claims = new[] { new Claim(ClaimTypes.Name, email), new Claim("StudentId", studentId.ToString()) };
            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLogout()
        {
            var user = Task.FromResult(new AuthenticationState(_anonymous));
            NotifyAuthenticationStateChanged(user);
        }
    }
}
