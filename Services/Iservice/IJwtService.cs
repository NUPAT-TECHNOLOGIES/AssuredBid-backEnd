namespace AssuredBid.Services.Iservice
{
    public interface IJwtService
    {
        string GenerateToken(string email);
        DateTime GetTokenExpiration(string token); // Add this method
    }
}
