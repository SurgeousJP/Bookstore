using Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Pages.Roles
{
    public class ListModel : AdminPageModel
    {
        public UserManager<ApplicationUser> UserManager;
        public RoleManager<IdentityRole> RoleManager;

        public ListModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }
        
        public IEnumerable<IdentityRole> Roles { get; set; } = Enumerable.Empty<IdentityRole>();

        public void OnGet()
        {
            // Why when convert ToList() the concurrency bug issues go away ?
            // Well it seems it is due to lazyloading, when there's no list, 
            // it returns an IQueryable and will not run until explicitly request it
            Roles = RoleManager.Roles.ToList();
        }

        public async Task<string> GetMembersString(string role)
        {
            IEnumerable<IdentityUser> users = (await UserManager.GetUsersInRoleAsync(role));

            string result = users.Count() == 0
                ? "No members available"
                : string.Join(", ", users.Take(3).Select(u => u.UserName).ToArray());

            return users.Count() > 3
                ? $"{result}, (plus others)"
                : result;
        }

        public async Task<IActionResult> OnPostAsync(string id) { 
            IdentityRole role = await RoleManager.FindByIdAsync(id);
            await RoleManager.DeleteAsync(role);
            return RedirectToPage();
        }
    }
}
