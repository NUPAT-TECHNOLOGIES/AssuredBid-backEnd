using AssuredBid.DTOs;

namespace AssuredBid.Services.Iservice
{
    public interface IRegistrationService
    {
        void Register(UserRegistrationDto dto);
        void Verify(VerifyCodeDto dto);
        string Login(LoginDto dto);

        // Password reset functionality
        void SendResetPasswordOtp(SendResetPasswordOtpDto dto);
        void VerifyResetPasswordOtp(VerifyResetPasswordOtpDto dto);
        void ResetPassword(ResetPasswordDto dto);

        // Logout method for token invalidation
        void Logout(string token);
    }
}
