using Microsoft.AspNetCore.Identity;

namespace BloggerBlazorServer.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = "Contributor";
    public bool IsAuth { get; set; } = false;
}

