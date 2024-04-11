using Identity.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Services
{
    public class ProfileService : IProfileService
    {
        private UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<string> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");

                return user.Id;
            }
            return "";
        }

        public async Task<IdentityResult> DeleteUserProfileAsync(string userId)
        {
            var existingUser = await _userManager.FindByIdAsync(userId);
            if (existingUser == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "User not found to delete"
                });
            }
            return await _userManager.DeleteAsync(existingUser);
        }

        public async Task<ApplicationUser?> GetUserProfileAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IdentityResult> UpdateUserProfileAsync(string userId, UserUpdateDTO updateUser)
        {
            var existingUser = await _userManager.FindByIdAsync(userId);

            if (existingUser == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "User not found"
                });
            }

            updateUser.setUpdateInfoToApplicationUser(existingUser);

            return await _userManager.UpdateAsync(existingUser);
        }
    }
}
