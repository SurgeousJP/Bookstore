using Identity.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Services
{
    public class ProfileService : IProfileService
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ProfileService> _logger;

        public ProfileService(UserManager<ApplicationUser> userManager, ILogger<ProfileService> logger, SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._logger = logger;
            _signInManager = signInManager;
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
            return string.Join(", ", result.Errors.Select(e => e.Description));
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

        public async Task<IdentityResult> UpdatePasswordAsync(string userId, string currentPassword, string newPassword)
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

            var result = await _userManager.ChangePasswordAsync(existingUser, currentPassword, newPassword);

            if (result.Succeeded)
            {
                _logger.LogInformation($"User {userId} has updated the password successfully");

                await _signInManager.RefreshSignInAsync(existingUser);

                return IdentityResult.Success;
            }

            _logger.LogInformation($"User {userId} failed to update the password");

            return IdentityResult.Failed(new IdentityError
            {
                Description = string.Join(", ", result.Errors.Select(e => e.Description))
            });
        }

        public async Task<string> GetResetToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return "";
            }

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "User with the provided email does not exist."
                });
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result;
        }

        public async Task<PaginatedItems<ApplicationUser>> GetUsersByPage(string searchString, int pageIndex = 0, int pageSize = 10)
        {
            var allUsersInRole = await _userManager.GetUsersInRoleAsync("Customer");

            // Filter users based on search string
            if (!string.IsNullOrEmpty(searchString))
            {
                allUsersInRole = allUsersInRole.Where(user =>
                    (!string.IsNullOrEmpty(user.UserName) && user.UserName.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(user.Email) && user.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(user.FullName) && user.FullName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            var totalUsersInRole = allUsersInRole.Count;
            var usersInRolePage = allUsersInRole
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedItems<ApplicationUser>(
                pageIndex,
                pageSize,
                totalUsersInRole,
                usersInRolePage
            );
        }
    }
}
