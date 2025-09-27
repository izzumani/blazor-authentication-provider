using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorProducts.Client.AuthProviders
{
    public class TestAuthStateProvider : AuthenticationStateProvider
    {
        private bool _isAuthenticated = false;
        private string _userRole = "User";

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.Delay(1500); // Simulate auth check delay

            if (_isAuthenticated)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "John Doe"),
                    new Claim(ClaimTypes.Role, _userRole)
                };

                var identity = new ClaimsIdentity(claims, "Test authentication");
                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            }
            else
            {
                // Return anonymous user
                var anonymous = new ClaimsIdentity();
                return new AuthenticationState(new ClaimsPrincipal(anonymous));
            }
        }

        // Methods to control auth state for testing
        public void Login(string role = "User")
        {
            _isAuthenticated = true;
            _userRole = role;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void Logout()
        {
            _isAuthenticated = false;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
