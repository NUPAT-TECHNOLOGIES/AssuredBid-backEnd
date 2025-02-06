using static System.Net.WebRequestMethods;

namespace AssuredBid.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? FirstName { get; set; } // Nullable
        public string? LastName { get; set; }  // Nullable
        public string? PhoneNumber { get; set; } // Nullable

        public bool IsVerified { get; set; }

        // Navigation property for OTP
        public Otp? Otp { get; set; }


        // Company information
        public string? CompanyName { get; set; } // Nullable
        public string? CompanyRegistrationNumber { get; set; } // Nullable
        public string? CompanyAddress { get; set; } // Nullable
        public string? StreetNumber { get; set; } // Nullable
        public string? City { get; set; } // Nullable
        public string? Country { get; set; } // Nullable
        public string? PostCode { get; set; } // Nullable
    }
}


