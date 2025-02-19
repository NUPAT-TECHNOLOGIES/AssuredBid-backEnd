using AssuredBid.Models;

namespace AssuredBid.Services.Iservice
{
    public interface ITenderService
    {
        Task<Tenders> GetTenderById(Guid id);
        Task<Tenders> AddTender(Tenders tender);
        Task<Tenders> UpdateTender(Tenders tender);
        Task<bool> DeleteTender(Guid id);
        Task<IEnumerable<Tenders>> GetAllTenders();
        Task<Notice> GetTendersByLimitsAndStages(int limit, string stages);
    }
}
