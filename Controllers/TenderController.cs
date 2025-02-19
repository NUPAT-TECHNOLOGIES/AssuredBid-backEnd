using AssuredBid.Services.Iservice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.IO;
using AssuredBid.Models;

namespace AssuredBid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenderController : ControllerBase
    {
        private readonly ITenderService tenderService;

        public TenderController(ITenderService tender)
        {
            this.tenderService = tender;
        }

        /// <summary>
        /// get tenders with limit = number of tenders to generate and stage = tender
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="stage"></param>
        /// <returns>The List of TENDERS</returns>
        [HttpGet]
        [Route("GetTendersWithLimitAndStages")]
        
        public async Task<IActionResult> GetTenders(int limit, string stage)
        {
            var response = await tenderService.GetTendersByLimitsAndStages(limit, stage);
            return Ok(response);
        }

        [HttpPost("CreateNewTender")]

        public async Task<IActionResult> CreateNewTenders([FromBody] Tenders tender)
        {
            var newTender = await 

        }

    }
}
