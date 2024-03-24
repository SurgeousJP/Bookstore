using Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.API.Pages.Users
{
    public class EditModel : AdminPageModel
    {
        public UserManager<ApplicationUser> UserManager;
        
        public EditModel(UserManager<ApplicationUser>  userManager)
        {
            UserManager = userManager;
        }
        [BindProperty] 
        public string Id { get; set; } = string.Empty;
        [BindProperty]
        public string UserName { get; set; } = string.Empty;
        [BindProperty]
        public string Email { get; set; } = string.Empty;
        [BindProperty]
        public string? Password { get; set; }

        public async Task OnGetAsync(string id)
        {
            ApplicationUser? user = await UserManager.FindByIdAsync(id);

            if (user != null)
            {
                Id = user.Id;
                UserName = user.UserName;
                Email = user.Email;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? user = await UserManager.FindByIdAsync(Id);
                user.UserName = UserName;
                user.Email = Email;
                IdentityResult result = await UserManager.UpdateAsync(user);
                if (result.Succeeded && !String.IsNullOrEmpty(Password))
                {
                    await UserManager.RemovePasswordAsync(user);
                    result = await UserManager.AddPasswordAsync(user, Password);
                }
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
