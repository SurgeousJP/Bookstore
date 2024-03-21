using Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.API.Pages.Roles
{
    public class EditModel : PageModel
    {
        public UserManager<ApplicationUser> UserManager;
        public RoleManager<IdentityRole> RoleManager;

        public EditModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public IdentityRole Role { get; set; } = new();

        public Task<IList<ApplicationUser>> Members() =>
            UserManager.GetUsersInRoleAsync(Role.Name);

        public async Task<IEnumerable<IdentityUser>> NonMembers() => UserManager.Users.ToList().Except(await Members());

        public async Task OnGetAsync(string id)
        {
            Role = await RoleManager.FindByIdAsync(id);
        }

        public async Task<IActionResult> OnPostAsync(string userId, string roleName)
        {
            Role = await RoleManager.FindByNameAsync(roleName);
            ApplicationUser user = await UserManager.FindByIdAsync(userId);
            IdentityResult result;
            if (await UserManager.IsInRoleAsync(user, roleName))
            {
                result = await UserManager.RemoveFromRoleAsync(user, roleName);
            }
            else
            {
                result = await UserManager.AddToRoleAsync(user, roleName);
            }
            if (result.Succeeded)
            {
                return RedirectToPage();
            }
            else
            {
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return Page();
        }
    }
}
