using Identity.API.Models;
using Identity.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private IProfileService profileService;
        private ILoginService<ApplicationUser> loginService;
        private IJwtBuilder jwtBuilder;
        private IEmailSender emailSender;
        private EmailConfiguration emailConfiguration;

        public UsersController(IProfileService profileService, ILoginService<ApplicationUser> loginService,
            IJwtBuilder jwtBuilder, IEmailSender emailSender, EmailConfiguration emailConfiguration)
        {
            this.profileService = profileService;
            this.loginService = loginService;
            this.jwtBuilder = jwtBuilder;
            this.emailSender = emailSender;
            this.emailConfiguration = emailConfiguration;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetUsersByPage([FromQuery] string? searchTerm, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var users = await profileService.GetUsersByPage(searchTerm, pageIndex, pageSize);

            return Ok(users);
        }

        [HttpPost("resetpass-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetPasswordResetToken([FromBody] string email)
        {
            var resetPassToken = await profileService.GetResetToken(email);
            if (string.IsNullOrEmpty(resetPassToken))
            {
                return BadRequest("The email input is not matched with any of users");
            }

            var message = new Message(new string[]
            { email },
            "Password recovery mail",
            "Dear [User],\r\nIt seems you've requested assistance in resetting your password. We're here to help you securely regain access to your account.." +
            $"\r\n Here is your token for reset your password: {resetPassToken}");

            await emailSender.SendEmailAsync(message);

            return Ok("A token is generated and has been sent to your email, please check your inbox for password recovery token");
        }

        [HttpPost("resetpass")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ResetPasswordForUser([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            var resetIdentityResult = await profileService.ResetPasswordAsync(resetPasswordDTO.Email, resetPasswordDTO.ResetToken, resetPasswordDTO.NewPassword);

            if (!resetIdentityResult.Succeeded)
            {
                string errorString = "";
                foreach (var error in resetIdentityResult.Errors)
                {
                    errorString = errorString + error + "\n";
                }
                return BadRequest($"User password change failed, error: {errorString}");
            }

            return Ok(resetIdentityResult);
        }

        [HttpPost("{userId}/update-password")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePasswordForUser([FromRoute] string userId, [FromBody] UpdatePasswordDTO updatePasswordDTO)
        {
            if (string.IsNullOrEmpty(userId) || updatePasswordDTO.isAnyAttributeNullOrEmpty())
            {
                return BadRequest("One or more attributes required is empty or null");
            }
            var currentUser = await profileService.GetUserProfileAsync(userId);

            if (currentUser == null)
            {
                return BadRequest("User id not found");
            }

            var checkPass = await loginService.ValidateCredentials(currentUser, updatePasswordDTO.CurrentPassword);

            if (!checkPass)
            {
                return BadRequest("User password not matched, please try again");
            }

            var updateIdentityResult = await profileService.UpdatePasswordAsync(userId, updatePasswordDTO.CurrentPassword, updatePasswordDTO.NewPassword);

            if (!updateIdentityResult.Succeeded)
            {
                string errorString = "";
                foreach (var error in updateIdentityResult.Errors)
                {
                    errorString = errorString + error.Description + "\n";
                }
                return BadRequest($"User password change failed, error: {errorString}");
            }

            return Ok(updateIdentityResult);
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

            var role = await loginService.SignIn(user);

            var token = jwtBuilder.GetToken(user.Id);

            return Ok(new
            {
                Message = $"The user {user.Id} is logged into the system",
                Token = token,
                Role = role
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutUser()
        {
            await loginService.SignOut();
            return Ok("The user has logged out successfully");
        }

        [HttpGet("validate")]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Validate([FromQuery(Name = "username")] string username, [FromQuery(Name = "token")] string token)
        {
            var user = await loginService.FindByUsername(username);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userId = jwtBuilder.ValidateToken(token);

            if (userId != user.Id)
            {
                return BadRequest("Invalid token, please login again");
            }

            return Ok(userId);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateUser([FromBody] UserRegisterDTO user)
        {
            if (user == null)
            {
                return BadRequest("User data not valid or empty");
            }

            var applicationUser = new ApplicationUser
            {
                UserName = user.Username,
                Email = user.Email,
            };

            var userId = await profileService.CreateUserAsync(applicationUser, user.Password);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Something wrong with creating user, please try again");
            }
            return Ok($"User with {user.Username} has been created, with userId: {userId}");
        }

        [HttpGet("profile/{userId}")]
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

        [HttpPatch("{userId}")]
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


        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteUserProfile([FromRoute] string userId)
        {
            var identityResult = await profileService.DeleteUserProfileAsync(userId);

            if (!identityResult.Succeeded)
            {
                string errorString = "";
                foreach (var error in identityResult.Errors)
                {
                    errorString = errorString + error + "\n";
                }
                return BadRequest($"User delete failed, error: {errorString}");
            }

            return Ok(identityResult);
        }
    }
}
