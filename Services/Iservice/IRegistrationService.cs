using AssuredBid.DTOs;

namespace AssuredBid.Services.Iservice
{
    public interface IRegistrationService
    {
        void Register(UserRegistrationDto dto);
        void Verify(VerifyCodeDto dto);
        void CompleteRegistration(CompleteRegistrationDto dto);
        string Login(LoginDto dto);

        // Add new methods for password reset functionality
        void SendResetPasswordOtp(SendResetPasswordOtpDto dto);
        void VerifyResetPasswordOtp(VerifyResetPasswordOtpDto dto);
        void ResetPassword(ResetPasswordDto dto);
    }
}
