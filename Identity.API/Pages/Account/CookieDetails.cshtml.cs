using Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging; // Import the ILogger namespace

namespace Identity.API.Pages.Account
{
    public class CookieDetailsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<CookieDetailsModel> logger; // Add ILogger<T> for logging

        public CookieDetailsModel(UserManager<ApplicationUser> userManager, ILogger<CookieDetailsModel> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        public IdentityUser? IdentityUser { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    // Log the fact that the user is authenticated
                    logger.LogInformation("User is authenticated.");

                    IdentityUser = await userManager.FindByNameAsync(User.Identity.Name);
                    if (IdentityUser != null)
                    {
                        // Log the user details
                        logger.LogInformation($"User found: {IdentityUser.UserName}");
                    }
                    else
                    {
                        // Log a warning if the user is not found
                        logger.LogWarning($"User not found with Id: {User.Identity.Name}");
                    }
                }
                else
                {
                    // Log a warning if the user is not authenticated
                    logger.LogWarning("User is not authenticated.");
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur
                logger.LogError(ex, "An error occurred while processing the request.");
            }
        }
    }
}
