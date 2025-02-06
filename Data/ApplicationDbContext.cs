using AssuredBid.Models;
using Microsoft.EntityFrameworkCore;

namespace AssuredBid.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Otp> Otps { get; set; }
        public DbSet<ResetPasswordOtp> ResetPasswordOtps { get; set; } // DbSet for ResetPasswordOtp,
    }
}
