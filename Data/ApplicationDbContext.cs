using AssuredBid.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssuredBid.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Otp> Otps { get; set; }
        public DbSet<ResetPasswordOtp> ResetPasswordOtps { get; set; }
        public DbSet<BlacklistedToken> BlacklistedTokens { get; set; }
        public DbSet<CreateTenders> tenders { get; set; }
        public DbSet<UsersPage> AdminUsers {  get; set; }
    }
}
