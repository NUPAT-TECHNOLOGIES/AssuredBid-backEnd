namespace AssuredBid.DTOs
{
    public class CompleteRegistrationDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        // Company information
        public string CompanyName { get; set; } 
        public string CompanyRegistrationNumber { get; set; } 
        public string CompanyAddress { get; set; } 
        public string StreetNumber { get; set; } 
        public string City { get; set; } 
        public string Country { get; set; } 
        public string PostCode { get; set; }
    }
}
