namespace AssuredBid.Models
{
    public class Otp
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // Foreign key to User
        public string Code { get; set; }
        public DateTime ExpirationTime { get; set; }  // OTP expiration time

        // Navigation property for User
        public User User { get; set; }
    }
}
