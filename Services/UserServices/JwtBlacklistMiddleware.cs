using AssuredBid.Data;

public class JwtBlacklistMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public JwtBlacklistMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Invoke(HttpContext context)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Perform your JWT blacklist check using dbContext
            // Example: var isBlacklisted = await dbContext.BlacklistedTokens.AnyAsync(t => t.Token == token);

            // Continue processing the request
            await _next(context);
        }
    }
}
