using System;
using BloggerBlazorServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BloggerBlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(UserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpPost("ChangeRole/{userId}")]
        public async Task<IActionResult> ChangeRole(string userId)
        {
            _logger.LogInformation("Changing role for user {UserId}", userId);
            var result = await _userService.ChangeUserRoleAsync(userId);
            if (!result)
            {
                _logger.LogWarning("Failed to change role for user {UserId}", userId);
                return BadRequest("Failed to change user role.");
            }
            return Ok();
        }


        [HttpPost("Authorize/{userId}")]
        public async Task<IActionResult> Authorize(string userId)
        {
            _logger.LogInformation("Authorizing user {UserId}", userId);
            var result = await _userService.AuthorizeUserAsync(userId);
            if (!result)
            {
                _logger.LogWarning("Failed to authorize user {UserId}", userId);
                return BadRequest("Failed to authorize user.");
            }
            return Ok();
        }

        [HttpPost("Deauthorize/{userId}")]
        public async Task<IActionResult> DeauthorizeUser(string userId)
        {
            var result = await _userService.DeauthorizeUserAsync(userId);
            if (!result)
            {
                return BadRequest("Failed to deauthorize user.");
            }
            return Ok();
        }

        [HttpGet("CurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userService.GetCurrentUserAsync();
            return Ok(user);
        }
    }
}