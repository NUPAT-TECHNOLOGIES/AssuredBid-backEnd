using AssuredBid.Models;

namespace AssuredBid.Services.Iservice
{
    public interface ITenderService
    {
        Task<Notice> GetTendersByLimitsAndStages(int limit, string stages);
    }
}
