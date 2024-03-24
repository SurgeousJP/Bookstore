using Identity.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Services
{
    public interface IProfileService
    {
        public Task<ApplicationUser?> GetUserProfileAsync(string userId);

        public Task<IdentityResult> UpdateUserProfileAsync(string userId, UserUpdateDTO user);
    }
}
