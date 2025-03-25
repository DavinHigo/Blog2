using System;
using BloggerBlazorServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using BloggerLibrary;

namespace BloggerBlazorServer.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<UserService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ApplicationUser>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> ChangeUserRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                return false;
            }

            var currentRole = user.Role;  // Get the current role of the user
            var newRole = string.Empty;

            if (currentRole == "Admin")
            {
                newRole = "Contributor";
            }
            else
            {
                newRole = "Admin";
            }

            user.Role = newRole;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                _logger.LogError("Failed to update role for user {UserId}", userId);
                return false;
            }

            _logger.LogInformation("Changed role of user {UserId} to {Role}", userId, newRole);
            return true;
        }

        public async Task<bool> AuthorizeUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                return false;
            }

            user.IsAuth = true;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Authorized user {UserId}", userId);
            return true;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext is null.");
            }

            var user = await _userManager.GetUserAsync(httpContext.User);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            return user;
        }

        public async Task<string> GetFullNameByEmailAsync(string email)
        {
            var user = await _context.Users
                .Where(u => u.Email == email)
                .Select(u => new { u.FirstName, u.LastName })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return $"{user.FirstName} {user.LastName}";
        }
    }
}
