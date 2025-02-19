namespace AssuredBid.AdminDto
{
    public class SendResetPasswordDto
    {
        public string Email { get; set; }
    }

    public class VerifyResetTokenDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class SetNewPasswordDto
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
