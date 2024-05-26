using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using WebApplication3.DTOs.Sponsor;
using WebApplication3.Entities;
using WebApplication3.repository.AccountRepository;
using WebApplication3.Service.AccountService;
using WebApplication3.Service.SponsorService;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SponsorController : Controller
    {
        private readonly ISponsorService sponsorService;
        public SponsorController(ISponsorService sponsorService)
        {
            this.sponsorService = sponsorService;
        }
        [HttpGet]
        public async Task<ActionResult<List<sponsor>>> GetAllSponsor()
        {
            var spon = await sponsorService.GetAllSponsor();
            return Ok(spon);
        }
        [HttpGet("{name}")]
        public async Task<ActionResult<sponsor>> GetSponsor(string name)
        {
            var spon = await sponsorService.GetSponsor(name);
            return Ok(spon);
        }
        [HttpPost("{Project_name}")]
        public async Task<ActionResult> AddSponsor(string Project_name,CreateRequestSponsorDTO create)
        {
            await sponsorService.AddSponsor(Project_name,create);
            return Ok(new { message = "sponsor created" });
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteSponsor(string name)
        {
            await sponsorService.DeleteSponsor(name);
            return Ok(new { message = "Sponsor deleted" });
        }
        [HttpPut]
        public async Task<ActionResult> UpdateSponsor(string name,UpdateRequestSponsorDTO up)
        {
            await sponsorService.UpdateSponsors(name,up);
            return Ok(new { message = "Sponsor Updated" });
        }


    }
}
