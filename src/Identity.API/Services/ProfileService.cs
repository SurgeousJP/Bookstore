using Identity.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Services
{
    public class ProfileService : IProfileService
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ProfileService> _logger;

        public ProfileService(UserManager<ApplicationUser> userManager, ILogger<ProfileService> logger)
        {
            this._userManager = userManager;
            this._logger = logger;
        }

        public async Task<string> CreateUserAsync(ApplicationUser user, string password)
        {
            _logger.LogInformation($"Began creating new user {user.UserName}");
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                _logger.LogInformation($"Successfully created new user {user.UserName} with role Customer");

                await _userManager.AddToRoleAsync(user, "Customer");

                return user.Id;
            }

            _logger.LogInformation($"Failed to create new user {user.UserName} with role Customer");
            return "";
        }

        public async Task<IdentityResult> DeleteUserProfileAsync(string userId)
        {
            var existingUser = await _userManager.FindByIdAsync(userId);
            if (existingUser == null)
            {
                _logger.LogInformation($"User {userId} not found for deletion");
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "User not found to delete"
                });
            }
            _logger.LogInformation($"Began deleting user {userId}");
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
                _logger.LogInformation($"User {userId} not found for updating");

                return IdentityResult.Failed(new IdentityError
                {
                    Description = "User not found"
                });
            }

            updateUser.setUpdateInfoToApplicationUser(existingUser);
            _logger.LogInformation($"Began updating user {userId}");
            
            return await _userManager.UpdateAsync(existingUser);
        }
    }
}
