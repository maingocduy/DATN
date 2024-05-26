using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTOs.Account;
using WebApplication3.DTOs.Member;
using WebApplication3.DTOs.Project;
using WebApplication3.Entities;
using WebApplication3.repository.AccountRepository;
using WebApplication3.Service.AccountService;
using WebApplication3.Service.MemberService;
using WebApplication3.Service.ProjectService;
using WebApplication3.Service.SponsorService;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService IMemberService;
        public MemberController(IMemberService IMemberService)
        {
            this.IMemberService = IMemberService;
        }
        [HttpPost]
        public async Task<IActionResult> Create(string name,CreateRequestMemberDTO mem)
        {
            await IMemberService.AddMember(name,mem);
            return Ok(new { message = "User created" });
        }
        [HttpGet]
        public async Task<ActionResult<List<Member>>> GetAllMember()
        {
            var mem = await IMemberService.GetAllMember();
            return Ok(mem);
        }
        [HttpGet("name")]
        public async Task<ActionResult<Member>> GetMember(string name)
        {
            var mem = await IMemberService.GetMember(name);
            return Ok(mem);
        }
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            await IMemberService.DeleteMember(name);
            return Ok(new { message = "Member deleted" });
        }
        [HttpPut]
        public async Task<ActionResult> UpdateMember(UpdateRequestMember up)
        {
            try
            {
                await IMemberService.UpdateMember(up);
                return Ok($"Member updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating member: {ex.Message}");
            };
        }

        [HttpPost("JoinProject")]
        public async Task<ActionResult> JoinProject(string Member_name,string project_name)
        {
            try
            {
                await IMemberService.JoinProject(project_name, Member_name);
                return Ok($"Join project successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error :{ex.Message}");
            };
        }
    }
}
