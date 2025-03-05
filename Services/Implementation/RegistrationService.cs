using AssuredBid.Data;
using AssuredBid.DTOs;
using AssuredBid.Models;
using AssuredBid.Services.Iservice;
using System.Security.Cryptography;

namespace AssuredBid.Services.UserServices
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly IJwtService _jwtService;

        public RegistrationService(ApplicationDbContext dbContext, IEmailService emailService, IJwtService jwtService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
            _jwtService = jwtService;
        }

        public void Register(UserRegistrationDto dto)
        {
            if (_dbContext.Users.Any(u => u.Email == dto.Email))
                throw new InvalidOperationException("Email already registered.");

            if (_dbContext.Users.Any(u => u.CompanyRegistrationNumber == dto.CompanyRegistrationNumber))
                throw new InvalidOperationException("Company registration number already exists.");


            var user = new User
            {
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                IsVerified = false,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                CompanyName = dto.CompanyName,
                CompanyRegistrationNumber = dto.CompanyRegistrationNumber,
                CompanyAddress = dto.CompanyAddress,
                StreetNumber = dto.StreetNumber,
                City = dto.City,
                Country = dto.Country,
                PostCode = dto.PostCode
            };
              
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            // Now send OTP after full details are provided
            var code = new Random().Next(100000, 999999).ToString();

            var otp = new Otp
            {
                UserId = user.Id,
                Code = code,
                ExpirationTime = DateTime.UtcNow.AddMinutes(5)
            };

            _dbContext.Otps.Add(otp);
            _dbContext.SaveChanges();

            _emailService.SendVerificationCode(dto.Email, code);
        }


        public void Verify(VerifyCodeDto dto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null || user.IsVerified)
                throw new InvalidOperationException("Invalid verification attempt.");

            var otp = _dbContext.Otps.FirstOrDefault(o => o.UserId == user.Id && o.Code == dto.Code);
            if (otp == null || otp.ExpirationTime < DateTime.Now)
                throw new InvalidOperationException("Invalid or expired OTP.");

            // Mark user as verified
            user.IsVerified = true;
            _dbContext.SaveChanges();

            // Remove OTP after verification
            _dbContext.Otps.Remove(otp);
            _dbContext.SaveChanges();
        }


        public string Login(LoginDto dto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null || !user.IsVerified || !VerifyPassword(dto.Password, user.PasswordHash))
                throw new InvalidOperationException("Invalid credentials.");

            return _jwtService.GenerateToken(user.Email);
        }

        // Methods for password reset functionality
        public void SendResetPasswordOtp(SendResetPasswordOtpDto dto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var code = new Random().Next(100000, 999999).ToString();

            var otp = new ResetPasswordOtp
            {
                Email = dto.Email,
                Otp = code,
                ExpirationTime = DateTime.UtcNow.AddMinutes(5) // Use UTC instead of local time
            };

            _dbContext.ResetPasswordOtps.Add(otp);
            _dbContext.SaveChanges();

            _emailService.SendResetPasswordOtp(dto.Email, code);
        }

        public void VerifyResetPasswordOtp(VerifyResetPasswordOtpDto dto)
        {
            var resetOtp = _dbContext.ResetPasswordOtps.FirstOrDefault(o => o.Email == dto.Email && o.Otp == dto.Otp);
            if (resetOtp == null || resetOtp.ExpirationTime < DateTime.Now)
                throw new InvalidOperationException("Invalid or expired OTP.");

            // OTP is valid, remove it to prevent reuse
            _dbContext.ResetPasswordOtps.Remove(resetOtp);
            _dbContext.SaveChanges();
        }

        public void ResetPassword(ResetPasswordDto dto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            user.PasswordHash = HashPassword(dto.NewPassword);
            _dbContext.SaveChanges();
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }

        public void Logout(string token)
        {
            var blacklistedToken = new BlacklistedToken
            {
                Token = token,
                Expiration = _jwtService.GetTokenExpiration(token)
            };

            _dbContext.BlacklistedTokens.Add(blacklistedToken);
            _dbContext.SaveChanges();
        }
    }
}
