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

        public async Task<List<TenderDTO>> GetTendersByLimits(int limit)
        {

            try
            {
                var client1 = httpClientFactory.CreateClient("Assured_bid");
                var client2 = httpClientFactory.CreateClient("Assured_bid2");
                var client3 = httpClientFactory.CreateClient("Assured_bid3");

                // Define API endpoints
                var response1 = $"ocdsReleasePackages?stages=tender&limit={limit}";
                var response2 = $"Published/Notices/OCDS/Search?stages=tender&limit={limit}";
                var response3 = $"Notices?noticeType=2&outputType=0&limit={limit}";

                // Fetch data concurrently
                var task1 = client1.GetStreamAsync(response1);
                var task2 = client2.GetStreamAsync(response2);
                var task3 = client3.GetStreamAsync(response3);

                // Wait for all tasks to complete
                var responses = await Task.WhenAll(task2);

                // Deserialize responses
                var tenders = new List<TenderDTO>();

                await Task.WhenAll(
                ProcessResponseAsync(task1.Result, tenders),
                ProcessResponseAsync(task2.Result, tenders),
                ProcessResponseAsync(task3.Result, tenders)
                );



                return tenders;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching tenders from multiple sources", ex);
            }
        }

        // ✅ Optimized response processing
        private async Task ProcessResponseAsync(Stream responseStream, List<TenderDTO> tenders)
        {
            if (responseStream == null) return;

            using var reader = new StreamReader(responseStream);
            using var jsonReader = new JsonTextReader(reader);
            var serializer = new JsonSerializer();

            var notice = serializer.Deserialize<Notice>(jsonReader);
            if (notice?.Releases == null) return;

            // Directly add results to the list (minimizing LINQ overhead)
            foreach (var r in notice.Releases)
            {
                if (r.Tender?.Classification?.Id?.Contains("85000000", StringComparison.OrdinalIgnoreCase) == true)
                {
                    tenders.Add(new TenderDTO
                    {
                        TenderId = r.Tender?.Id ?? "N/A",
                        TenderTitle = r.Tender?.Title ?? "No Title",
                        Status = r.Tender?.Status ?? "Unknown",
                        Description = r.Tender?.Description ?? "Null",
                        DeadLine = r.Tender?.TenderPeriod?.EndDate ?? DateTime.MinValue,
                        Category = r.Tender?.Classification?.Description ?? "Unknown",
                        Type = r.Tender?.MainProcurementCategory ?? "Unknown",
                        Budget = r.Tender?.Value?.Amount ?? 0m,
                        ClassificationId = r.Tender?.Classification?.Id ?? "Unknown",
                        ClassificationScheme = r.Tender?.Classification?.Scheme ?? "Unknown"
                    });
                }
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
