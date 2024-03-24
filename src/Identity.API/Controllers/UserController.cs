using Identity.API.Models;
using Identity.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Identity.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private IProfileService profileService;
        private ILoginService<ApplicationUser> loginService;

        public UserController(IProfileService profileService, ILoginService<ApplicationUser> loginService)
        {
            this.profileService = profileService;
            this.loginService = loginService;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDTO loginDTO)
        {
            // Check nullable fields
            if (loginDTO.Username == null || loginDTO.Password == null)
            {
                return BadRequest("Missing username or password");
            }
            // Check exist user
            var user = await loginService.FindByUsername(loginDTO.Username);
            if (user == null)
            {
                return BadRequest("Username not existed, please try again");
            }
            // Validate user
            var validationStatus = await loginService.ValidateCredentials(user, loginDTO.Password);
            if (!validationStatus)
            {
                return BadRequest("The password is incorrect, please try again");
            }

            await loginService.SignIn(user);
            return Ok($"The user {user.Id} is logged into the system");
        }

        [HttpGet("get/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetUserProfile([FromRoute] string userId)
        {
            var userProfile = await profileService.GetUserProfileAsync(userId);
            if (userProfile == null)
            {
                return BadRequest("UserId not exists");
            }
            return Ok(userProfile);
        }

        [HttpPatch("update/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateUserProfile([FromRoute] string userId, [FromBody] UserUpdateDTO updateDTO)
        {
            var updateIdentityResult = await profileService.UpdateUserProfileAsync(userId, updateDTO);

            if (!updateIdentityResult.Succeeded)
            {
                string errorString = "";
                foreach (var error in updateIdentityResult.Errors)
                {
                    errorString = errorString + error + "\n";
                }
                return BadRequest($"User update failed, error: {errorString}");
            }

            return Ok(updateIdentityResult);
        }
    }
}
