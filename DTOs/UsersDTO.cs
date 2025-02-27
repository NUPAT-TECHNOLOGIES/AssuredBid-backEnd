using System.ComponentModel.DataAnnotations;

namespace AssuredBid.DTOs
{
    public class UsersDTO
    {
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; }
        public DateTime LastActive { get; set; }
    }
}
