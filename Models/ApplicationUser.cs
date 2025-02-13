using Microsoft.AspNetCore.Identity;

namespace AssuredBid.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<string> Roles { get; set; }
    }
}
