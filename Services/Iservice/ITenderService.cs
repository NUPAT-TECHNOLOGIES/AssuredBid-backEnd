using AssuredBid.DTOs;
using AssuredBid.Models;

namespace AssuredBid.Services.Iservice
{
    public interface ITenderService
    {
        Task<CreateTenders> GetTenderById(Guid id);
        Task<CreateTenderDTO> AddTender(CreateTenderDTO tender);
        Task<CreateTenders> UpdateTender(CreateTenders tender);
        Task<bool> DeleteTender(Guid id);
        Task<IEnumerable<CreateTenders>> GetAllTenders();
        Task<Notice> GetTendersByLimitsAndStages(int limit, string stages);
    }
}
