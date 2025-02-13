namespace AssuredBid.Models
{
    //Registration of superadmin
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsSuperAdmin { get; set; }
    }

    //Login superadmin
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
