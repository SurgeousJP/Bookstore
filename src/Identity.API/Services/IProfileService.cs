using Identity.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Services
{
    public interface IProfileService
    {
        public Task<string> CreateUserAsync(ApplicationUser user, string password);
        public Task<ApplicationUser?> GetUserProfileAsync(string userId);

        public Task<IdentityResult> UpdateUserProfileAsync(string userId, UserUpdateDTO user);

        public Task<IdentityResult> DeleteUserProfileAsync(string userId);

        public Task<IdentityResult> UpdatePasswordAsync(string userId, string currentPassword, string newPassword);

        public Task<string> GetResetToken(string email);
        public Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword);
    }
}
