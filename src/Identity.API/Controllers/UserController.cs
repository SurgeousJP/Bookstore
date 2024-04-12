﻿using Identity.API.Models;
using Identity.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Identity.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private IProfileService profileService;
        private ILoginService<ApplicationUser> loginService;
        private IJwtBuilder jwtBuilder;

        public UserController(IProfileService profileService, ILoginService<ApplicationUser> loginService,
            IJwtBuilder jwtBuilder)
        {
            this.profileService = profileService;
            this.loginService = loginService;
            this.jwtBuilder = jwtBuilder;
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

            //await loginService.SignIn(user);

            var token = jwtBuilder.GetToken(user.Id);

            return Ok(new
            {
                Message = $"The user {user.Id} is logged into the system",
                Token = token
            });
        }

        [Authorize]
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
        public async Task<IActionResult> Validate([FromQuery(Name = "username")] string username, [FromQuery(Name= "token")] string token)
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

        [Authorize]
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

        [Authorize]
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

        [Authorize(Roles="Admin")]
        [HttpDelete("delete/{userId}")]
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