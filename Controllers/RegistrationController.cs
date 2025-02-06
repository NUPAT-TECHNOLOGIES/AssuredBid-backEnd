using AssuredBid.DTOs;
using AssuredBid.Services.Iservice;
using Microsoft.AspNetCore.Mvc;

namespace AssuredBid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegistrationDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid registration data.");

            try
            {
                _registrationService.Register(dto);
                return Ok("Verification code sent to email.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("verify")]
        public IActionResult Verify([FromBody] VerifyCodeDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid verification data.");

            try
            {
                _registrationService.Verify(dto);
                return Ok("Email verified. You can now complete registration.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("complete-registration")]
        public IActionResult CompleteRegistration([FromBody] CompleteRegistrationDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid completion data.");

            try
            {
                _registrationService.CompleteRegistration(dto);
                return Ok("Registration completed successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid login data.");

            try
            {
                var token = _registrationService.Login(dto);
                return Ok(new { Token = token });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // New endpoint to send the OTP for password reset
        [HttpPost("send-reset-password-otp")]
        public IActionResult SendResetPasswordOtp([FromBody] SendResetPasswordOtpDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Email))
                return BadRequest("Invalid reset password data.");

            try
            {
                _registrationService.SendResetPasswordOtp(dto);
                return Ok("Password reset OTP sent to email.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // New endpoint to verify the OTP for password reset
        [HttpPost("verify-reset-otp")]
        public IActionResult VerifyResetOtp([FromBody] VerifyResetPasswordOtpDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Otp))
                return BadRequest("Invalid OTP verification data.");

            try
            {
                _registrationService.VerifyResetPasswordOtp(dto);
                return Ok("OTP verified. You can now reset your password.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // New endpoint to reset the password after OTP verification
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.NewPassword))
                return BadRequest("Invalid password reset data.");

            try
            {
                _registrationService.ResetPassword(dto);
                return Ok("Password reset successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
