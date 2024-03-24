using Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.API.Pages.Roles
{
    public class CreateModel : PageModel
    {
        public RoleManager<IdentityRole> RoleManager;

        public CreateModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            RoleManager = roleManager;
        }

        [BindProperty]
        public string Name { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole { Name = Name };
                IdentityResult result = await RoleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToPage("roles/list");
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
