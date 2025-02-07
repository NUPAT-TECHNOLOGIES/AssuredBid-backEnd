using AssuredBid.Services.Iservice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.IO;

namespace AssuredBid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenderController : ControllerBase
    {
        private readonly ITenderService tender;

        public TenderController(ITenderService tender)
        {
            this.tender = tender;
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
            var response = await tender.GetTendersByLimitsAndStages(limit, stage);
            return Ok(response);
        }
    }
}
