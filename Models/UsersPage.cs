using System.ComponentModel.DataAnnotations;

namespace AssuredBid.Models
{
    public class UsersPage
    {
        [Key]
        public Guid UserId { get; set; }
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;

    }
}
