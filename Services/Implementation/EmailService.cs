using AssuredBid.Services.Iservice;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;

namespace AssuredBid.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _emailFrom;
        private readonly string _emailPassword;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _smtpHost = configuration["EmailSettings:SmtpServer"] ?? throw new ArgumentNullException("SMTP server is not configured.");
            _smtpPort = int.TryParse(configuration["EmailSettings:SmtpPort"], out int port) ? port : 465;  // Default to 465 if not provided
            _emailFrom = configuration["EmailSettings:FromEmail"] ?? throw new ArgumentNullException("From email is not configured.");
            _emailPassword = configuration["EmailSettings:SmtpPassword"] ?? throw new ArgumentNullException("SMTP password is not configured.");
            _logger = logger;
        }

        public void SendVerificationCode(string toEmail, string code)
        {
            var subject = "Your Verification Code";
            var body = $"<p>Your verification code is: <strong>{code}</strong></p>";

            try
            {
                // Create a MimeMessage
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("AssuredBid", _emailFrom));
                emailMessage.To.Add(new MailboxAddress("", toEmail));
                emailMessage.Subject = subject;

                // Build the email body
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body // Use HTML for better formatting
                };
                emailMessage.Body = bodyBuilder.ToMessageBody();

                // Send the email using MailKit
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Connect(_smtpHost, _smtpPort, MailKit.Security.SecureSocketOptions.SslOnConnect); // Use SSL for port 465
                    smtpClient.Authenticate(_emailFrom, _emailPassword);
                    smtpClient.Send(emailMessage);
                    smtpClient.Disconnect(true);
                }

                // Log email success
                _logger.LogInformation($"Verification code sent to {toEmail}.");
            }
            catch (Exception ex)
            {
                // Log the error and throw it
                _logger.LogError($"Failed to send verification code to {toEmail}: {ex.Message}");
                throw new InvalidOperationException($"Failed to send email: {ex.Message}", ex);
            }
        }

        // Updated method for sending reset password OTP with the modified message format
        public void SendResetPasswordOtp(string toEmail, string otp)
        {
            var subject = "Password Reset OTP";
            var body = $"<p>You your password reset OTP is: <strong>{otp}</strong></p>"; // Updated message format

            try
            {
                // Create a MimeMessage
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("AssuredBid", _emailFrom));
                emailMessage.To.Add(new MailboxAddress("", toEmail));
                emailMessage.Subject = subject;

                // Build the email body
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body // Use HTML for better formatting
                };
                emailMessage.Body = bodyBuilder.ToMessageBody();

                // Send the email using MailKit
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Connect(_smtpHost, _smtpPort, MailKit.Security.SecureSocketOptions.SslOnConnect); // Use SSL for port 465
                    smtpClient.Authenticate(_emailFrom, _emailPassword);
                    smtpClient.Send(emailMessage);
                    smtpClient.Disconnect(true);
                }

                // Log email success
                _logger.LogInformation($"Password reset OTP sent to {toEmail}.");
            }
            catch (Exception ex)
            {
                // Log the error and throw it
                _logger.LogError($"Failed to send reset password OTP to {toEmail}: {ex.Message}");
                throw new InvalidOperationException($"Failed to send email: {ex.Message}", ex);
            }
        }
    }
}
