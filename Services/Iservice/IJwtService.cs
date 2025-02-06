namespace AssuredBid.Services.Iservice
{
    public interface IJwtService
    {
        string GenerateToken(string email);
    }
}
