using AssuredBid.DTOs;
using AssuredBid.Services.Iservice;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Registers a new user and sends a verification code to their email.
        /// </summary>
        /// <param name="dto">User registration details.</param>
        /// <returns>A confirmation message.</returns>
        /// <response code="200">Verification code sent successfully.</response>
        /// <response code="400">Invalid registration data.</response>
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

        /// <summary>
        /// Verifies the email using the provided verification code.
        /// </summary>
        /// <param name="dto">Verification code details.</param>
        /// <returns>A confirmation message.</returns>
        /// <response code="200">Email verified successfully.</response>
        /// <response code="400">Invalid verification data.</response>
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

        /// <summary>
        /// Completes user registration after email verification.
        /// </summary>
        /// <param name="dto">User completion details.</param>
        /// <returns>A success message.</returns>
        /// <response code="200">Registration completed successfully.</response>
        /// <response code="400">Invalid completion data.</response>
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

        /// <summary>
        /// Logs in the user and returns an authentication token.
        /// </summary>
        /// <param name="dto">Login details.</param>
        /// <returns>JWT token.</returns>
        /// <response code="200">Login successful.</response>
        /// <response code="400">Invalid login data.</response>
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

        /// <summary>
        /// Sends an OTP to the user's email for password reset.
        /// </summary>
        /// <param name="dto">User email details.</param>
        /// <returns>A confirmation message.</returns>
        /// <response code="200">OTP sent successfully.</response>
        /// <response code="400">Invalid reset password data.</response>
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

        /// <summary>
        /// Verifies the OTP for password reset.
        /// </summary>
        /// <param name="dto">OTP verification details.</param>
        /// <returns>A success message.</returns>
        /// <response code="200">OTP verified successfully.</response>
        /// <response code="400">Invalid OTP verification data.</response>
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

        /// <summary>
        /// Resets the user's password after OTP verification.
        /// </summary>
        /// <param name="dto">New password details.</param>
        /// <returns>A success message.</returns>
        /// <response code="200">Password reset successfully.</response>
        /// <response code="400">Invalid password reset data.</response>
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


        /// <summary>
        /// Logs out the user by blacklisting the current JWT token.
        /// </summary>
        /// <returns>A confirmation message.</returns>
        /// <response code="200">Logout successful.</response>
        /// <response code="400">No token provided.</response>
        /// <response code="401">Token is invalid or already blacklisted.</response>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
                return BadRequest("No token provided.");

            try
            {
                _registrationService.Logout(token);
                return Ok("Logged out successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


    }
}
