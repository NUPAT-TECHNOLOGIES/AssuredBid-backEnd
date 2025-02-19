using AssuredBid.AdminDto;
using AssuredBid.DTOs;
using AssuredBid.Models;
using AssuredBid.Services.Iservice;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace AssuredBid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IRegistrationService _registrationService;
        private readonly IJwtService _jwtService;

        // Stores unverified admins temporarily (Email -> (Token, Password, User))
        private static readonly ConcurrentDictionary<string, (string Token, string Password, ApplicationUser User)> _unverifiedAdmins
            = new ConcurrentDictionary<string, (string, string, ApplicationUser)>();


        // Stores reset tokens (Email -> Token)
        private static readonly ConcurrentDictionary<string, string> _passwordResetTokens = new ConcurrentDictionary<string, string>();

        // Stores verified emails (Email -> true)
        private static readonly ConcurrentDictionary<string, bool> _verifiedEmails = new ConcurrentDictionary<string, bool>();






        public AdminController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            RoleManager<IdentityRole> roleManager,
            IRegistrationService registrationService,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _registrationService = registrationService;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        /// <summary>
        /// Registers an admin and sends a verification token.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                return BadRequest("An admin with this email already exists.");

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            // Generate a random 6-digit token
            var token = new Random().Next(100000, 999999).ToString();

            // Store token, password, and user temporarily
            _unverifiedAdmins[model.Email] = (token, model.Password, user);

            // Send the token via email
            _emailService.SendVerificationCode(model.Email, token);

            return Ok(new { message = "Verification token sent to email. Complete verification to finalize registration." });
        }

        /// <summary>
        /// Verifies the admin with the token and finalizes registration.
        /// </summary>
        [HttpPost("verify-admin")]
        public async Task<IActionResult> VerifyAdmin([FromBody] VerifyAdminDto dto)
        {
            if (!_unverifiedAdmins.TryGetValue(dto.Email, out var storedData))
                return BadRequest("No pending registration found for this email.");

            if (storedData.Token != dto.Token)
                return BadRequest("Invalid verification token.");

            var user = storedData.User;
            var password = storedData.Password;

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                _unverifiedAdmins.TryRemove(dto.Email, out _);
                return Ok(new { message = "Admin verified and registered successfully." });
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Logs in an admin.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
                return Unauthorized("Invalid email or password.");

            var token = _jwtService.GenerateToken(user.Email);
            return Ok(new { message = "Login successful.", token });
        }

        /// <summary>
        /// Logs out the admin.
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logged out successfully." });
        }

        /// <summary>
        /// Sends a reset password token to the admin's email.
        /// </summary>
        [HttpPost("send-reset-password-token")]
        public async Task<IActionResult> SendResetPasswordToken([FromBody] SendResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return BadRequest("Admin with this email does not exist.");

            // Generate a random 6-digit token
            var token = new Random().Next(100000, 999999).ToString();

            // Store the token
            _passwordResetTokens[dto.Email] = token;

            // Send token via email
            _emailService.SendResetPasswordOtp(dto.Email, token);

            return Ok(new { message = "Reset password token sent to your email." });
        }

        /// <summary>
        /// Verifies the token before allowing password reset.
        /// </summary>
        [HttpPost("verify-reset-token")]
        public IActionResult VerifyResetToken([FromBody] VerifyResetTokenDto dto)
        {
            if (!_passwordResetTokens.TryGetValue(dto.Email, out var storedToken))
                return BadRequest("No reset request found for this email.");

            if (storedToken != dto.Token)
                return BadRequest("Invalid or expired reset token.");

            // Mark email as verified
            _verifiedEmails[dto.Email] = true;

            return Ok(new { message = "Token verified successfully. You can now set a new password." });
        }

        /// <summary>
        /// Allows setting a new password only if the token was verified.
        /// </summary>
        [HttpPost("set-new-password")]
        public async Task<IActionResult> SetNewPassword([FromBody] SetNewPasswordDto dto)
        {
            if (!_verifiedEmails.ContainsKey(dto.Email) || !_verifiedEmails[dto.Email])
                return BadRequest("Token not verified. Please verify your token first.");

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return BadRequest("Admin with this email does not exist.");

            // Reset password
            var resetResult = await _userManager.RemovePasswordAsync(user);
            if (!resetResult.Succeeded)
                return BadRequest("Error removing old password.");

            var setPasswordResult = await _userManager.AddPasswordAsync(user, dto.NewPassword);
            if (!setPasswordResult.Succeeded)
                return BadRequest("Error setting new password.");

            // Clean up data after successful reset
            _passwordResetTokens.TryRemove(dto.Email, out _);
            _verifiedEmails.TryRemove(dto.Email, out _);

            return Ok(new { message = "Password reset successfully." });
        }
    }

}
