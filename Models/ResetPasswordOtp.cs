namespace AssuredBid.Models
{
    public class ResetPasswordOtp
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Otp { get; set; }
        public DateTime ExpirationTime { get; set; } // When the OTP will expire
    }
}
