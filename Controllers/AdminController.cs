using AssuredBid.DTOs;
using AssuredBid.Models;
using AssuredBid.Services.Iservice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AssuredBid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRegistrationService _registrationService;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IRegistrationService registrationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _registrationService = registrationService;
        }

        // Register a new user with the option to make them a super admin
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (model.IsSuperAdmin)
                {
                    if (!await _roleManager.RoleExistsAsync(Roles.SuperAdmin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin));
                    }
                    await _userManager.AddToRoleAsync(user, Roles.SuperAdmin);
                }
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(new { message = "Super-Admin created successfully." });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }

        // Login for existing users
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(new { message = "Logged in successfully." });
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return BadRequest(ModelState);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logged out successfully." });
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

