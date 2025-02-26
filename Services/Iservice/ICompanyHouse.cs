using AssuredBid.Models;

namespace AssuredBid.Services.Iservice
{
    public interface ICompanyHouse
    {
        Task<CompanyProfile> GetCompanyProfile(string number);
    }
}
