using Identity.API.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Services
{
    public class LoginService : ILoginService<ApplicationUser>
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;

        public LoginService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> _userManager)
        {
            this._signInManager = signInManager;
            this._userManager = _userManager;
        }

        public async Task<ApplicationUser> FindByUsername(string username)
        {
            return await _signInManager.UserManager.FindByNameAsync(username);
        }

        public Task<bool> ValidateCredentials(ApplicationUser user, string password)
        {
            return _userManager.CheckPasswordAsync(user, password);
        }

        public async Task SignIn(ApplicationUser user)
        {
            var authProperties = new AuthenticationProperties
            {
               ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
               IsPersistent = true,
               RedirectUri = "To be added later on"
            };
            await SignInAsync(user, authProperties);
        }

        public Task SignInAsync(ApplicationUser user, AuthenticationProperties properties, string authenticationMethod = null)
        {
            return _signInManager.SignInAsync(user, properties, authenticationMethod);
        }
    }
}
