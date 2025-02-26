using AssuredBid.Models;
using AssuredBid.Services.Iservice;
using Newtonsoft.Json;

namespace AssuredBid.Services.UserServices
{
    public class CompanyHouseService : ICompanyHouse
    {
        private readonly IHttpClientFactory httpClientFactory;

        public CompanyHouseService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<CompanyProfile> GetCompanyProfile(string number)
        {
            var verify = httpClientFactory.CreateClient("Company_House");
            try
            {
                var response = await verify.GetAsync($"company/{number}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var tenderResponse = JsonConvert.DeserializeObject<CompanyProfile>(data);
                        return tenderResponse;
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                else
                {
                    response.StatusCode.ToString();
                    return new CompanyProfile();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
