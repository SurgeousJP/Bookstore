using Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.API.Pages.Users
{
    public class ListModel : AdminPageModel
    {
        public UserManager<ApplicationUser> UserManager;
        public ListModel(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }
        public IEnumerable<ApplicationUser> Users { get; set; }
        = Enumerable.Empty<ApplicationUser>();
        public void OnGet()
        {
            Users = UserManager.Users;
        }
        public async Task<IActionResult> OnPostAsync(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null) {
                await UserManager.DeleteAsync(user);
            }
            return RedirectToPage();
        }
    }
}
