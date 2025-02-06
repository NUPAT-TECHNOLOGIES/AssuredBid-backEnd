namespace AssuredBid.Services.Iservice
{
    public interface IEmailService
    {
        void SendVerificationCode(string toEmail, string code);

        // Method for sending reset password OTP
        void SendResetPasswordOtp(string toEmail, string otp);
    }
}
