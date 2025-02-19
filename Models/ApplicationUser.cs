using Microsoft.AspNetCore.Identity;

namespace AssuredBid.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsVerified { get; set; } = false; // Default to false
    }
}
