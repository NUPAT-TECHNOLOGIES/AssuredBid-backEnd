using AssuredBid.Data;
using AssuredBid.DTOs;
using AssuredBid.Mappings;
using AssuredBid.Models;
using AssuredBid.Services.Iservice;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AssuredBid.Services.UserServices
{
    public class TenderService : ITenderService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;
        public TenderService(IHttpClientFactory httpClientFactory, ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            this.httpClientFactory = httpClientFactory;
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
        }

        public async Task<CreateTenderDTO> AddTender(CreateTenderDTO tenderDto)
        {
            var tender = mapper.Map<CreateTenders>(tenderDto);
            applicationDbContext.tenders.Add(tender);
            await applicationDbContext.SaveChangesAsync();
            return tenderDto;
        }

        public async Task<bool> DeleteTender(Guid id)
        {
            var tender = await applicationDbContext.tenders.FindAsync(id);
            if (tender == null) return false;

            applicationDbContext.tenders.Remove(tender);
            await applicationDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CreateTenders>> GetAllTenders()
        {
            return await applicationDbContext.tenders.ToListAsync();
        }

        public async Task<CreateTenders> GetTenderById(Guid id)
        {
            return await applicationDbContext.tenders.FirstAsync();
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

        public async Task<CreateTenders> UpdateTender(CreateTenders tender)
        {
            applicationDbContext.tenders.Update(tender);
            await applicationDbContext.SaveChangesAsync();
            return tender;
        }
    }
}
