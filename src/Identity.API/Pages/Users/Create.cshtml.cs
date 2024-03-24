using Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Identity.API.Pages.Users
{
    public class CreateModel : AdminPageModel
    {
        public UserManager<ApplicationUser> UserManager;

        public CreateModel(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }
        [BindProperty] 
        public string UserName { get; set; } = string.Empty;
        [BindProperty]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [BindProperty]
        public string Password {  get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user =
                    new ApplicationUser { UserName = UserName, Email = Email };
                IdentityResult result =
                    await UserManager.CreateAsync(user, Password);
                if (result.Succeeded)
                {
                    return RedirectToPage("/users/list");
                }
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return Page();
        }
    }
}

