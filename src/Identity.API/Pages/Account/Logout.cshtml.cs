using Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.API.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private SignInManager<ApplicationUser> signInManager;

        public LogoutModel(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
        }
        public async Task OnGetAsync()
        {
            await signInManager.SignOutAsync();
        }
    }
}
