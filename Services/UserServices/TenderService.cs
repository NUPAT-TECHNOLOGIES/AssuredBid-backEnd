using AssuredBid.Models;
using AssuredBid.Services.Iservice;
using Newtonsoft.Json;

namespace AssuredBid.Services.UserServices
{
    public class TenderService : ITenderService
    {
        private readonly IHttpClientFactory httpClientFactory;
        public TenderService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<Notice> GetTendersByLimitsAndStages(int limit, string stages)
        {
            var verify = httpClientFactory.CreateClient("Assured_bid");
            try
            {
                var response = await verify.GetAsync($"ocdsReleasePackages?limit={limit}&stages={stages}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var tenderResponse = JsonConvert.DeserializeObject<Notice>(data);
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
                    return new Notice();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
