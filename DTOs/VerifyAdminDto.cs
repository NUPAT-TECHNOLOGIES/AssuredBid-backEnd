namespace AssuredBid.DTOs
{
    public class VerifyAdminDto
    {
        public string Email { get; set; }  // Ensure this matches what you send from the client
        
        public string Token { get; set; } // Token from email
       
    }
}
